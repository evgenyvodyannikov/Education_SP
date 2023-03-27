function executeFullAdvertisement() {

    let fullAdvertisement = {

        //#region Variables and conditions

        id: null,
        listItemType: null,
        currentUserId: _spPageContextInfo.userId,
        webAbsoluteUrl: _spPageContextInfo.webAbsoluteUrl,
        isCurrentUserAdmin: false,
        currentStatus: '',
        isAuthor: false,
        APIEndpoint: '',

        //#endregion

        //#region Functions

        //#region Utility

        GetAttachmentsHTML: function (attachmentsUrls) {

            let html = '';

            attachmentsUrls.forEach(url => {
                html += `
                <div class="col-sm-4 col-xs-12">
                    <a href="${url}">
                        <img src="${url}" alt>
                    </a>
                </div>`
            });

            return html;

        },

        //#endregion

        //#region Manage Advertisement

        publishAdvertisement: function () {

            const requestUrl = fullAdvertisement.APIEndpoint;

            let patchStatus = fullAdvertisement.currentStatus == 'Active' ? 'Moderation' : 'Active';

            $.ajax({
                url: requestUrl,
                type: "POST",
                contentType: "application/json;odata=verbose",
                data: JSON.stringify({
                    "__metadata": {
                        "type": "SP.Data.AdvertisementsListItem"
                    },
                    "Status": patchStatus
                }),
                headers: {
                    "X-RequestDigest": $('#__REQUESTDIGEST').val(),
                    "Accept": "application/json;odata=verbose",
                    "X-HTTP-Method": "MERGE",
                    "If-Match": "*"
                },
                success: function (data) {
                    console.log("Item updated successfully");
                    window.location.reload();
                },
                error: function (error) {
                    console.error("Error updating item: " + JSON.stringify(error));
                }
            });
        },

        deleteAdvertisement: function () {

            const requestUrl = fullAdvertisement.APIEndpoint;

            isAccepted = confirm('Вы уверены, что хотите удалить текущее объявление?');

            if (isAccepted) {
                $.ajax({
                    url: requestUrl,
                    method: "DELETE",
                    headers: {
                        "X-RequestDigest": $('#__REQUESTDIGEST').val(),
                        "X-HTTP-Method": "MERGE",
                        "If-Match": "*",
                        "Content-Type": "application/json;odata=verbose"
                    },
                    success: function (data) {
                        alert("Объявление успешно удалено");
                        window.location.href = `${fullAdvertisement.webAbsoluteUrl}`
                    },
                    error: function (error) {
                        console.error("Error updating item: " + JSON.stringify(error));
                    }
                });
            }
        },

        //#endregion

        fillAdvertisementData: function (data) {

            fullAdvertisement.isAuthor = fullAdvertisement.currentUserId == data.CreatedById;

            if (fullAdvertisement.isCurrentUserAdmin) {
                $('#admin-panel').removeClass('inactive');
                $('.aside-box.birthday-box').remove();
            }
            else if (fullAdvertisement.isAuthor) {
                $('#admin-panel').removeClass('inactive');
                $('#changeStatus').remove();
                $('.aside-box.birthday-box').remove();
            }

            let title = $('div.container > div.title');
            let text = $('div.text > p');
            let categoryDateInfo = $('div.hh > div.date');
            let authorLink = $('div.hh > div.who > a')
            let authorImage = $('div.hh > div.who > a > div.img img');
            let authorName = $('div.hh > div.who > a > div.name');
            let attachments = $('div.row.desk-photos')
            let linkToUser = $('a.link')
            let status = $('.aside-title.title#status');
            let changeBtn = $('#changeStatus');

            let advertisementUrl = `${fullAdvertisement.webAbsoluteUrl}/Pages/EditAdvertisement.aspx?$id=${fullAdvertisement.id}`;

            $('#sitePath > li > a').attr("href", fullAdvertisement.webAbsoluteUrl);
            $('#sitePath').append(`<li><a href="${advertisementUrl}">${data.Title}</a></li>`);

            let userLink = getUserLink(data.CreatedById);
            let userInfo = getUserInfo(data.CreatedById);

            let dateCreated = data.Created.replace(/\D/g, '');
            dateCreated = getLocalDate(dateCreated * 1, true);

            title.html(data.Title);
            document.title = data.Title;
            text.html(data.Description || '');

            categoryDateInfo.html(`${dateCreated} · ${data.Category.Title}`)

            authorImage[0].src = userInfo.Image;
            authorName.html(userInfo.Name);
            authorLink[0].href = userLink;

            if (data.Attachments) {
                imageSet = getAttachmentUrls(fullAdvertisement.webAbsoluteUrl, data.Id);
                attachments.html(fullAdvertisement.GetAttachmentsHTML(imageSet))
            }

            linkToUser.html(`Написать ${userInfo.Name}`)
            linkToUser[0].href = userLink;

            let statusRu = data.Status.Value == 'Active' ? 'активно' : 'на модерации'

            // Admin-panel 

            status.html(`Объявление ${statusRu}`);
            if (data.Status.Value == 'Active') {
                changeBtn.html('На модерацию');
            }
            else {
                changeBtn.html('Опубликовать');
            }
            $('a#edit').attr("href", advertisementUrl);
            fullAdvertisement.currentStatus = data.Status.Value;

        },

        loadAdvertisement: function () {

            fullAdvertisement.isCurrentUserAdmin = isGroupMember(_spPageContextInfo.webAbsoluteUrl, fullAdvertisement.currentUserId, "marketplace Owners");
            fullAdvertisement.id = getQueryStringParameter("$id", window.location.search.substring(1)) || null;
            fullAdvertisement.APIEndpoint = `${fullAdvertisement.webAbsoluteUrl}/_api/web/lists/getbytitle('Advertisements')/Items(${fullAdvertisement.id})`;

            if (fullAdvertisement.id !== null) {

                let filter = `$filter = Id eq ${fullAdvertisement.id}`;
                let fields = '$select = Id, Title, Description, CreatedById, Created, Attachments, Status/Value, Category/Title&$expand=Category, Status'
                const requestUrl = `${fullAdvertisement.webAbsoluteUrl}/_vti_bin/listdata.svc/Advertisements?${filter}&${fields}`;

                $.ajax({
                    url: requestUrl,
                    type: "GET",
                    headers: {
                        "accept": "application/json;odata=verbose"
                    },
                    async: false,
                    success: function (data) {
                        fullAdvertisement.fillAdvertisementData(data.d.results[0]);
                    },
                    error: function (err) {
                        console.log("There was an error" + err);
                        $('div.container > div.title').html('Ошибка. Повторите попытку позже...');
                    }
                });
            }
            else {
                $('div.container > div.title').html('Ошибка. Повторите попытку позже...');
            }
        },

        //#endregion
    }

    fullAdvertisement.loadAdvertisement();

    //#region Event Handlers

    $('#admin-panel').on('click', '#changeStatus', function () {
        fullAdvertisement.publishAdvertisement();
    });

    $('#admin-panel').on('click', '#delete', function () {
        fullAdvertisement.deleteAdvertisement();
    });

    //#endregion
}

$(document).ready(function () {

    if ($("#MSOLayout_InDesignMode").attr('value') != "1") {
        updatePageProperties(document, 'Объявление');
        SP.SOD.executeFunc("sp.js", "SP.ClientContext", executeFullAdvertisement)
    }

});