﻿
    function changed() {
        $("#searchForm").submit();
        cleanUserDetails();
        $("#results").empty().addClass("loader");
    }

    function hideLoader() {
        $("#user-details").removeClass("loader");
        $("#results").removeClass("loader");
    }

    function getUserDetails() {
        cleanUserDetails();
        $("#user-details").addClass("loader");
    }

    function cleanUserDetails() {
        $("#results").empty();
        $("#user-details").empty();
    }
    function userDetailsLoaded() {
        hideLoader();
        
    }
