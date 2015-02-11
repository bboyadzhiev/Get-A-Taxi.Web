using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Get_A_Taxi.Web.Infrastructure.ModelBinders
{
    public class DoubleModelBinder : IModelBinder
    {
        //public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    ValueProviderResult valueResult = bindingContext.ValueProvider
        //        .GetValue(bindingContext.ModelName);

        //    ModelState modelState = new ModelState { Value = valueResult };

        //    object actualValue = null;

        //    if (valueResult.AttemptedValue != string.Empty)
        //    {
        //        try
        //        {
        //            actualValue = Convert.ToDouble(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
        //        }
        //        catch (FormatException e)
        //        {
        //            modelState.Errors.Add(e);
        //        }
        //    }

        //    bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

        //    return actualValue;
        //}
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (string.IsNullOrEmpty(valueResult.AttemptedValue))
            {
                return 0m;
            }
            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDouble(
                    valueResult.AttemptedValue.Replace(",", "."),
                    CultureInfo.InvariantCulture
                );
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}