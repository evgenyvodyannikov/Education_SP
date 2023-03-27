export const getUserInfo = (userID) => {   

    const requestURL = `${_spPageContextInfo.webAbsoluteUrl}/_api/web/getuserbyid('${userID}')`;
    
    let userName = undefined;
    let loginName = undefined;
    let userImage = undefined;
    let imageSize = 'L';

    $.ajax({      
        url: requestURL,      
        type: "GET",      
        headers: {      
            "Accept": "application/json; odata=verbose"      
        },   
        async: false,       
        success: function(data) {     
            userName = data.d.Title;  
            loginName = data.d.LoginName;     
        },      
        error: function(err) {      
            console.log("There was an error" + err);     
        }      
    });

    userImage = `${_spPageContextInfo.siteAbsoluteUrl}/_layouts/15/userphoto.aspx?size=${imageSize}&accountname=${loginName}`

    return {
        Name: userName, 
        Image: userImage,
    };
}  