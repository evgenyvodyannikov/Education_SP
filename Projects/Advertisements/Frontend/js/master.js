function executeMasterPage() {

    let masterPage = {
        currentUserName: '',
        userImageUrl: '',

        displayUserInfo: function (userData) {

            $('div#userName').html(userData.Name);
            $('img#userImage').attr('src', userData.Image);

        },

        loadUserName: function () {

            let currentUserId = _spPageContextInfo.userId;
            let userData = getUserInfo(currentUserId);

            this.displayUserInfo(userData);
        }
    }

    masterPage.loadUserName();
}

$(document).ready(function () {

    if ($("#MSOLayout_InDesignMode").attr('value') != "1") {
        SP.SOD.executeFunc("sp.js", "SP.ClientContext", executeMasterPage)
    }

});