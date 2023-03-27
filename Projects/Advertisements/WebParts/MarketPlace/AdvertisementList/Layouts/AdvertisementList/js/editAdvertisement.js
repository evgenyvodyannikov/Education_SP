function executeEditAdvertisement() {

    let EditAdvertisement = {

        //#region Variables and conditions

        webAbsoluteUrl: _spPageContextInfo.webAbsoluteUrl,
        currentUserId: _spPageContextInfo.userId,
        isAdmin: false,
        listItemType: null,
        isAuthor: false,
        selectedCategoryId: 0,
        attachments: [],
        currentStatus: 'Moderation',
        id: null,
        dropzone: null,
        itemCount: 0,
        isNewItem: false,

        //#endregion

        //#region Functions

        //#region Utility

        getAPIEndpointV10: function (listName = 'Advertisements') {
            return `${EditAdvertisement.webAbsoluteUrl}/_vti_bin/listdata.svc/${listName}`;
        },

            getAPIEndpointV13: function (id = null, listName = 'Advertisements') {
                if (id) {
                    return `${EditAdvertisement.webAbsoluteUrl}/_api/web/lists/getByTitle('${listName}')/items('${id}')`;
                }
                else {
                    return `${EditAdvertisement.webAbsoluteUrl}/_api/web/lists/getByTitle('${listName}')/items`;
                }
            },

                getServiceAPIEndpoint: function () {
                    return `${EditAdvertisement.webAbsoluteUrl}/_layouts/15/MarketPlace.Services.FileManager/FileManager.svc/test`;
                },

                validate: function () {

                    let pass = true;
                    let message = '';

                    if ($('.edit-title').val().length < 3) {
                        pass = false;
                        message += 'Заголовок должен содержать больше 3 символов\n'
                    }

                    if (EditAdvertisement.itemCount < 1) {
                        pass = false;
                        message += 'Вы должны загрузить хотя бы 1 фото'
                    }

                    if (message.length > 0) {
                        alert(message);
                    }

                    return pass;

                },

                //#endregion

                //#region Manage Attachments

                selectFiles: function () {

                    let files = $('#file4')[0].files;

                    $.each(files, function (index, file) {
                        if (EditAdvertisement.attachments.filter(item => !item.MarkToDelete).length < 3) {

                            let imageType = /^image\//;

                            if (imageType.test(file.type)) {

                                EditAdvertisement.attachments.push({
                                    Url: URL.createObjectURL(file),
                                    IsAttachment: true,
                                    FileName: file.name,
                                    MarkToDelete: false,
                                    isNew: true,
                                    Data: file
                                });

                            }
                        }
                        else {
                            alert('Вы не можете добавить более 3 картинок');
                        }

                    });
                    EditAdvertisement.updateImagesLayout();

                },

                deleteImage: function (index) {
                    EditAdvertisement.attachments[index].MarkToDelete = true;
                    $(`ul.download-images li#${index}`).remove();
                },

                deleteMarkedAttachments: function () {

                    let deletedAttachments = EditAdvertisement.attachments.filter(item => item.MarkToDelete && !item.isNew)

                    $.each(deletedAttachments, function (index, item) {

                        let requestUrl = `${EditAdvertisement.getServiceAPIEndpoint()}/DeleteFile(${EditAdvertisement.id},${item.FileName})`

                        $.ajax({
                            url: requestUrl,
                            method: "DELETE",
                            headers: {
                                "X-RequestDigest": $('#__REQUESTDIGEST').val(),
                                "X-HTTP-Method": "DELETE",
                                "If-Match": "*",
                                "Content-Type": "application/json;odata=verbose"
                            },
                            success: function (data) {
                                if (data.DeleteFileResult) {
                                    console.log("Item delete successfully");
                                }
                                console.log("Item is not deleted successfully");
                            },
                            error: function (error) {
                                console.error("Error updating item: " + JSON.stringify(error));
                            }
                        });

                    });

                },

                createNewAttachments: function () {

                    let newAttachments = EditAdvertisement.attachments.filter(item => item.isNew && !item.MarkToDelete);

                    $.each(newAttachments, function (index, item) {

                        let requestUrl = `${EditAdvertisement.getServiceAPIEndpoint()}/AddFile(${EditAdvertisement.id},${item.FileName})`;
                        let reader = new FileReader();
                        reader.onload = function (e) {
                            let fileData = e.target.result;
                            $.ajax({
                                url: requestUrl,
                                method: "POST",
                                data: fileData,
                                processData: false,
                                headers: {
                                    "accept": "application/json;odata=verbose",
                                    "X-RequestDigest": $("#__REQUESTDIGEST").val(),
                                },
                                success: function (data) {
                                    console.log(data);
                                    console.log("File uploaded successfully");
                                },
                                error: function (error) {
                                    console.log("Error uploading file: " + error);
                                }
                            });
                        };
                        reader.readAsArrayBuffer(item.Data);

                    });

                },

                saveImages: function () {

                    EditAdvertisement.deleteMarkedAttachments();
                    EditAdvertisement.createNewAttachments();

                },

                updateImagesLayout: function () {

                    let imageContainer = $('ul.download-images');
                    imageContainer.html('');

                    let activeAttachments = EditAdvertisement.attachments.filter(item => !item.MarkToDelete);

                    $.each(activeAttachments, function (index, item) {
                        imageContainer.append(`
                <li id="${index}">
                    <img src="${item.Url}" alt="">
                    <img class="delete" src="${EditAdvertisement.webAbsoluteUrl}/_layouts/15/AdvertisementList/img/x.svg" alt="delete-icon">
                </li>
                `)
                    });

                },

                loadImages: function () {

                    const requestURL = `${EditAdvertisement.getServiceAPIEndpoint()}/GetFilesByItemId(${EditAdvertisement.id})`;

                    $.ajax({
                        url: requestURL,
                        type: "GET",
                        headers: {
                            "accept": "application/json;odata=verbose"
                        },
                        async: false,
                        success: function (data) {
                            $.each(data.d, function (index, item) {
                                let mockFile = { name: item.Name, size: 12345 };
                                EditAdvertisement.dropzone.displayExistingFile(mockFile, item.Url);
                            });
                            EditAdvertisement.itemCount = data.d.length;
                        },
                        error: function (err) {
                            console.log("There was an error" + err);
                        }
                    });
                },

                //#endregion

                //#region Manage Categories

                selectCategory: function () {
                    let selectedValue = $("Select#Categories option:selected").val();
                    selectedCategoryId = selectedValue;
                },

                fillCategories: function (items) {

                    let optionsContainer = $('#Categories');
                    let optionsHTML = '';

                    $.each(items, function (index, item) {
                        let selectedAttr = ''
                        if (item.Id == EditAdvertisement.selectedCategoryId) {
                            selectedAttr = 'selected';
                        }
                        optionsHTML += `<option value="${item.Id}" ${selectedAttr}>${item.Title}</option>`;
                    });
                    optionsContainer.html(optionsHTML);

                    jcf.customForms.replaceAll();

                },

                displayCategories: function (categoryId = 1) {

                    EditAdvertisement.selectedCategoryId = categoryId;

                    const categoriesAPIEndpoind = EditAdvertisement.getAPIEndpointV10('Categories');
                    const requestURL = categoriesAPIEndpoind;

                    $.ajax({
                        url: requestURL,
                        type: "GET",
                        headers: {
                            "accept": "application/json;odata=verbose"
                        },
                        success: function (data) {
                            EditAdvertisement.fillCategories(data.d.results);
                        },
                        error: function (err) {
                            console.log("There was an error" + err);
                        }
                    });

                },

                    //#endregion

                    //#region Manage Advertisements

                    publishAdvertisement: function () {

                        const requestUrl = getAPIEndpointV13(EditAdvertisement.id);

                        let patchStatus = EditAdvertisement.currentStatus == 'Active' ? 'Moderation' : 'Active';

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

                    saveAdvertisement: function () {

                        if (EditAdvertisement.validate()) {

                            $('div.container > div.title').html('Успешно. Переадресация...');
                            $('div.container#main').addClass('inactive');

                            let patchTitle = $('.edit-title').val();
                            let patchDescription = $('.edit-description').val();
                            let patchCategory = EditAdvertisement.selectedCategoryId;
                            let patchStatus = 'Moderation';

                            if (EditAdvertisement.id) {

                                const requestUrl = `${EditAdvertisement.getServiceAPIEndpoint()}/EditAdvertisement`;

                                let updateItem = {
                                    "Id": Number(EditAdvertisement.id),
                                    "Title": patchTitle,
                                    "Description": patchDescription,
                                    "CategoryId": patchCategory,
                                    "Status": patchStatus,
                                    "Attachments": [],
                                };

                                $.ajax({
                                    url: requestUrl,
                                    type: "POST",
                                    contentType: "application/json;odata=verbose",
                                    async: false,
                                    data: JSON.stringify(updateItem),
                                    headers: {
                                        "X-RequestDigest": $('#__REQUESTDIGEST').val(),
                                        "Accept": "application/json;odata=verbose",
                                    },
                                    success: function (data) {
                                        console.log(data);
                                        console.log("Item updated successfully");
                                    },
                                    error: function (error) {
                                        console.error("Error updating item: " + JSON.stringify(error));
                                    }
                                });

                            }
                            else {

                                let title = $('.edit-title').val();
                                let description = $('.edit-description').val();
                                let categoryId = EditAdvertisement.selectedCategoryId;
                                let status = 'Moderation';
                                let authorId = EditAdvertisement.currentUserId;

                                const requestUrl = `${EditAdvertisement.getServiceAPIEndpoint()}/CreateAdvertisement`;
                                console.log(requestUrl);
                                let attachments = [];
                                let files = $('#dz')[0].dropzone.files;

                                $.each(files, function (index, item) {
                                    let str = item.dataURL;
                                    let base64 = str.substr(str.indexOf("base64,") + 7, str.length);
                                    attachments.push({ "Name": item.name, "Data": base64 })
                                });

                                let updateItem = {
                                    "Title": title,
                                    "Description": description,
                                    "CategoryId": categoryId,
                                    "Status": status,
                                    "AuthorId": authorId,
                                    "Attachments": attachments,
                                };

                                $.ajax({
                                    url: requestUrl,
                                    method: 'POST',
                                    contentType: "application/json;odata=verbose",
                                    headers: {
                                        "X-RequestDigest": $('#__REQUESTDIGEST').val(),
                                        "Accept": "application/json;odata=verbose",
                                    },
                                    async: false,
                                    data: JSON.stringify(updateItem),
                                    success: function (data) {
                                        EditAdvertisement.id = data.d.Id;
                                        console.log('New item created with ID ' + EditAdvertisement.id);

                                    },
                                    error: function (error) {
                                        console.log('Error creating item: ' + JSON.stringify(error));
                                    }
                                });
                            }

                            // EditAdvertisement.saveImages();
                            window.location.href = `${EditAdvertisement.webAbsoluteUrl}/Pages/FullAdvertisement.aspx?$id=${EditAdvertisement.id}`

                        }

                    },

                    deleteAdvertisement: function () {

                        const requestUrl = EditAdvertisement.getAPIEndpointV13(id);

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
                                    alert("Item delete successfully");
                                    window.location.href = `${EditAdvertisement.webAbsoluteUrl}`
                                },
                                error: function (error) {
                                    console.error("Error updating item: " + JSON.stringify(error));
                                }
                            });
                        }
                    },

                    fillAdvertisementData: function (data) {

                        let title = $('.edit-title');
                        let description = $('.edit-description');
                        let status = $('.aside-title.title#status');
                        let changeBtn = $('#changeStatus');

                        $('#sitePath').append(`<li><a href="${EditAdvertisement.webAbsoluteUrl}/Pages/EditAdvertisement.aspx?$id=${EditAdvertisement.id}">${data.Title}</a></li>`);

                        title.val(data.Title);
                        description.val(data.Description);

                        // Admin-panel 
                        EditAdvertisement.currentStatus = data.Status.Value;

                        let statusRu = EditAdvertisement.currentStatus == 'Active' ? 'активно' : 'на модерации';

                        status.html(`Объявление ${statusRu}`);
                        if (EditAdvertisement.currentStatus == 'Active') {
                            changeBtn.html('На модерацию');
                        }
                        else {
                            changeBtn.html('Опубликовать');
                        }

                        if (data.Attachments) {
                            EditAdvertisement.loadImages();
                        }

                    },

                    displayAdvertisementInfo: function () {

                        let filter = `$filter = Id eq ${EditAdvertisement.id}`;
                        let fields = '$select = Id, Title, Description, CreatedById, Created, Attachments, Status/Value, CategoryId&$expand=Status'

                        const advertisementsAPIEndpoind = EditAdvertisement.getAPIEndpointV10();
                        const requestUrl = `${advertisementsAPIEndpoind}?${filter}&${fields}`;

                        $.ajax({
                            url: requestUrl,
                            type: "GET",
                            headers: {
                                "accept": "application/json;odata=verbose"
                            },
                            async: false,
                            success: function (data) {

                                let items = data.d.results;

                                if (items.length != 0) {

                                    EditAdvertisement.isAuthor = EditAdvertisement.currentUserId == items[0].CreatedById;

                                    if (EditAdvertisement.isAdmin || EditAdvertisement.isAuthor) {

                                        // Admin-panel
                                        $('#admin-panel').removeClass('inactive');
                                        $('.aside-box.birthday-box').remove();
                                        $('div.container#main').removeClass('inactive');

                                        if (!EditAdvertisement.isAdmin) {
                                            // Status btn
                                            $('#changeStatus').remove();
                                        }

                                        EditAdvertisement.displayCategories(items[0].CategoryId);
                                        EditAdvertisement.fillAdvertisementData(items[0]);
                                    }
                                    else {
                                        $('div.container > div.title').html("Недостаточно прав. Попробуйте позже...");
                                    }
                                }
                                else {
                                    let message = 'Недостаточно прав. Попробуйте позже...';
                                    if (EditAdvertisement.isAdmin) {
                                        message = `Объявление с id = ${id} не существует...`
                                    }
                                    $('div.container > div.title').html(message);
                                }

                            },
                            error: function (err) {
                                console.log("There was an error" + err);
                                let message = 'Недостаточно прав. Попробуйте позже...';
                                if (EditAdvertisement.isAdmin) {
                                    message = `Произошла ошибка при получении данных...`
                                }
                                $('div.container > div.title').html(message);
                            }
                        });

                    },

                    //#endregion

                    //#region Dropzone

                    removeFile: function (file) {

                        if (!EditAdvertisement.isNewItem) {

                            let requestUrl = `${EditAdvertisement.getServiceAPIEndpoint()}/DeleteFiles`
                            let array = [];

                            let requestItem = {
                                "AdvertisementId": EditAdvertisement.id,
                                "Name": file.name,
                            }
                            array.push(requestItem);

                            $.ajax({
                                url: requestUrl,
                                method: "DELETE",
                                contentType: "application/json;odata=verbose",
                                data: JSON.stringify(array),
                                headers: {
                                    "X-RequestDigest": $('#__REQUESTDIGEST').val(),
                                    "Accept": "application/json;odata=verbose",
                                },
                                success: function (data) {
                                    console.log("Item delete successfully");
                                },
                                error: function (error) {
                                    console.error("Error deleting item: " + JSON.stringify(error));
                                }
                            });

                        }

                    },

                    onUploadDone: function (file) {

                        if (!EditAdvertisement.isNewItem) {

                            console.log(file);

                            let advertisementId = Number(EditAdvertisement.id);
                            let fileName = file.name;
                            let str = file.dataURL;
                            let base64 = str.substring(str.indexOf("base64,") + 7, str.length);

                            let array = [];
                            let requestItem = {
                                "Id": 0,
                                "AdvertisementId": advertisementId,
                                "Name": fileName,
                                "Data": base64,
                            }

                            array.push(requestItem);

                            let requestUrl = `${EditAdvertisement.getServiceAPIEndpoint()}/AddFiles`;

                            $.ajax({
                                url: requestUrl,
                                type: "POST",
                                contentType: "application/json;odata=verbose",
                                async: false,
                                data: JSON.stringify(array),
                                headers: {
                                    "X-RequestDigest": $('#__REQUESTDIGEST').val(),
                                    "Accept": "application/json;odata=verbose",
                                },
                                success: function (data) {
                                    console.log(data);
                                    console.log("File uploaded successfully");
                                },
                                error: function (error) {
                                    EditAdvertisement.dropzone.removeFile(file);
                                    alert('Изображение уже добавлено!')
                                }
                            });

                        }

                    },

                    loadDropzone: function () {

                        let dz = new Dropzone($('#dz')[0], {
                            url: window.location.href,
                            autoProcessQueue: true,
                            uploadMultiple: true,
                            dictDefaultMessage: 'Перетащите файлы или нажмите...',
                            acceptedFiles: 'image/jpeg,image/png,image/gif,image/jpg',
                            addRemoveLinks: true,
                            dictRemoveFile: "×",
                            maxFiles: 3,
                        });

                        EditAdvertisement.dropzone = dz;

                        dz.on("addedfile", (file) => {
                            if (EditAdvertisement.itemCount < 3) {
                                EditAdvertisement.itemCount += 1;
                            }
                            else {
                                alert('Вы добавили маскимум изображений!')
                                EditAdvertisement.dropzone.removeFile(file);
                            }
                        });

                        dz.on("success", (file) => {
                            EditAdvertisement.onUploadDone(file);
                        });

                        dz.on("removedfile", function (file) {
                            EditAdvertisement.removeFile(file);
                            EditAdvertisement.itemCount -= 1;
                        });

                    },

                    //#endregion

                    loadAdvertisement: function () {

                        EditAdvertisement.isAdmin = isGroupMember(EditAdvertisement.webAbsoluteUrl, EditAdvertisement.currentUserId, "marketplace Owners");
                        EditAdvertisement.id = getQueryStringParameter("$id", window.location.search.substring(1)) || null;
                        $('#sitePath > li > a').attr("href", EditAdvertisement.webAbsoluteUrl);

                        EditAdvertisement.loadDropzone();

                        if (EditAdvertisement.id) {
                            // edit
                            $('div.container > div.title').html('Изменить объявление');
                            EditAdvertisement.displayAdvertisementInfo();
                            document.title = 'Редактировать объявление'
                        }
                        else {
                            // create
                            $('div.container > div.title').html('Создать объявление');
                            $('div.container#main').removeClass('inactive');
                            $('#sitePath').append(`<li><a href="#">Новое объявление</a></li>`);
                            document.title = 'Создать объявление'
                            EditAdvertisement.isNewItem = true;
                            EditAdvertisement.displayCategories();
                        }

                    },

                    //#endregion

                    }

                EditAdvertisement.loadAdvertisement();

                //#region Event Handlers

                $('.file').on('change', '#file4', function () {
                    EditAdvertisement.selectFiles();
                });

                $('.row-inp').on('click', '.btn.publish', function () {
                    EditAdvertisement.saveAdvertisement();
                });

                $('.download-images').on('click', ".delete", function () {

                    let item = $(this);
                    let index = $(item[0].parentElement)[0].id;

                    EditAdvertisement.deleteImage(index);
                });

                //#endregion

            }

            $(document).ready(function () {

                if ($("#MSOLayout_InDesignMode").attr('value') != "1") {
                    updatePageProperties(document);
                    SP.SOD.executeFunc("sp.js", "SP.ClientContext", executeEditAdvertisement)
                }

            });

            const getServiceAPIEndpoint = () => {
                return `${_spPageContextInfo.webAbsoluteUrl}/_layouts/15/MarketPlace.Services.FileManager/FileManager.svc/test/`;
            }

            const createAdv = () => {
                const requestUrl = `${getServiceAPIEndpoint()}/CreateAdvertisement`;

                let attachments = [];

                let files = $('#dz')[0].dropzone.files;

                $.each(files, function (index, item) {

                    let str = item.dataURL;
                    let base64 = str.substr(str.indexOf("base64,") + 7, str.length);
                    attachments.push({ "Name": item.name, "Data": base64 })
                });

                console.log(attachments);

                $.ajax({
                    url: requestUrl,
                    type: "POST",
                    contentType: "application/json;odata=verbose",
                    async: false,
                    data: JSON.stringify({
                        "Title": 'newItem',
                        "Description": 'sdssds',
                        "Status": 'Active',
                        "CategoryId": 1,
                        "AuthorId": 18,
                        "Attachments": attachments
                    }),
                    headers: {
                        "X-RequestDigest": $('#__REQUESTDIGEST').val(),
                        "Accept": "application/json;odata=verbose",
                    },
                    success: function (data) {
                        console.log(data);
                        console.log("Item updated successfully");
                    },
                    error: function (error) {
                        console.error("Error updating item: " + JSON.stringify(error));
                    }
                });

            }