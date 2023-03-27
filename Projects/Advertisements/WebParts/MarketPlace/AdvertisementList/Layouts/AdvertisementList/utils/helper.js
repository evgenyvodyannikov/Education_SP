const getUserInfo = (userID) => {

    let userName = undefined;
    let userImage = undefined;
    let loginName = undefined;

    let requestURL = `${_spPageContextInfo.webAbsoluteUrl}/_api/web/getuserbyid('${userID}')`;


    $.ajax({
        url: requestURL,
        type: "GET",
        headers: {
            "Accept": "application/json; odata=verbose"
        },
        async: false,
        success: function (data) {
            loginName = data.d.LoginName;
        },
        error: function (err) {
            console.log("There was an error" + err);
        }
    });

    requestURL = `${_spPageContextInfo.webAbsoluteUrl}/_api/sp.userprofiles.peoplemanager/GetPropertiesFor(accountName=@v)?@v='${encodeURIComponent(loginName)}'`;

    $.ajax({
        url: requestURL,
        type: "GET",
        headers: {
            "Accept": "application/json; odata=verbose"
        },  
        async: false,
        success: function (data) {
            let properties = data.d;    
            userName = properties.DisplayName;
            userImage = properties.PictureUrl;
            if (!userImage) {
                userImage = `${_spPageContextInfo.webAbsoluteUrl}/_layouts/15/AdvertisementList/img/user.png`
            }
        },
        error: function (err) {
            console.log("There was an error" + err);
        }
    });

    return {
        Name: userName,
        Image: userImage,
    };
}

const getUserLink = (userID) => {
    return `${_spPageContextInfo.webAbsoluteUrl}/_layouts/15/userdisp.aspx?ID=${userID}`
}

const getLocalDate = (date, year = false) => {

    let localDate = new Date(date);
    let dateOptions;

    if (year) {
        dateOptions = { year: 'numeric', month: 'long', day: 'numeric' };
    }
    else {
        dateOptions = { day: 'numeric', month: 'long' };
    }

    return localDate.toLocaleDateString("ru-RU", dateOptions);

}

const isGroupMember = (webUrl, userId, groupName) => {

    let isAdmin = false;
    const requestUrl = `${webUrl}/_api/web/getuserbyid('${userId}')/groups`

    $.ajax({
        url: requestUrl,
        type: "GET",
        headers: {
            "Accept": "application/json; odata=verbose"
        },
        async: false,
        success: function (data) {

            $.each(data.d.results, function (index, item) {
                if (item.Title == groupName) {
                    isAdmin = true;
                    return;
                }
            });

        },
        error: function (err) {
            console.log("There was an error" + err);
        }
    });

    return isAdmin;

};

const getQueryStringParameter = (param, query) => {

    let vars = query.split('&');
    for (let i = 0; i < vars.length; i++) {
        let pair = vars[i].split('=');
        if (decodeURIComponent(pair[0]) === param) {
            return decodeURIComponent(pair[1]);
        }
    }
    return null;
}

const getAttachmentUrls = (webAbsoluteUrl, id, FirstItem = false) => {

    let attachmentsUrls = [];
    const requestURL = `${webAbsoluteUrl}/_vti_bin/listdata.svc/Advertisements(${id})/Attachments`

    $.ajax({
        url: requestURL,
        type: "GET",
        headers: {
            "accept": "application/json;odata=verbose"
        },
        async: false,
        success: function (data) {
            $.each(data.d.results, function (index, item) {
                attachmentsUrls.push(item.__metadata.media_src);
            });
        },
        error: function (err) {
            console.log("There was an error" + err);
        }
    });


    if(attachmentsUrls.length == 0){
        attachmentsUrls.push('/_catalogs/masterpage/_layouts/15/AdvertisementList/img/no-data.jpg');
    }

    if (FirstItem) {
        return attachmentsUrls[0];
    }
    else {
        return attachmentsUrls;
    }
}

const updatePageProperties = (context, title = null) => {

    let icon = context.createElement('link');
    icon.rel = 'icon';
    icon.href="../_layouts/15/AdvertisementList/img/marketplace.png";
    context.head.append(icon);

    if(title){
        context.title = title;
    }

}