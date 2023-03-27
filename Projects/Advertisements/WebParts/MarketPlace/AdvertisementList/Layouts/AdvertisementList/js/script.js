function executeProductCatalog() {

    let productCatalog = {


        //#region Variables and conditions

        currentPage: 1,
        webAbsoluteUrl: _spPageContextInfo.webAbsoluteUrl,
        pageCount: 0,
        pageSize: 5,
        currentUserId: _spPageContextInfo.userId,
        isCurrentUserAdmin: false,
        selectedCategoryId: 0,
        isLookingActiveAds: true,
        isLookingUserAds: false,

        //#endregion


        displayCategories: function () {

            const categoriesAPIEndpoind = productCatalog.getAPIEndpoint('Categories');
            const requestURL = categoriesAPIEndpoind;

            $.ajax({
                url: requestURL,
                type: "GET",
                headers: {
                    "accept": "application/json;odata=verbose"
                },
                success: function (data) {
                    productCatalog.fillCategories(data.d.results);
                },
                error: function (err) {
                    console.log("There was an error" + err);
                }
            });
        },

        fillCategories: function (items) {

            let optionsContainer = $('#Categories');
            let optionsHTML = `<option value="0">Любая</option>`;

            $.each(items, function (index, item) {
                optionsHTML += `<option value="${item.Id}">${item.Title}</option>`;
            });
            optionsContainer.html(optionsHTML);

            jcf.customForms.replaceAll()
        },

        getAPIEndpoint: function (listName) {
            return `${_spPageContextInfo.webAbsoluteUrl}/_vti_bin/listdata.svc/${listName}`
        },

        getListProperty: function(listName, itemId, propertyName) {

            const requestURL = `${_spPageContextInfo.webAbsoluteUrl}/_api/web/lists/getbytitle('${listName}')/items(${itemId})/?$select=${propertyName}`;
            let result = undefined;
    
            $.ajax({
                url: requestURL,
                type: "GET",
                headers: {
                    "Accept": "application/json; odata=verbose"
                },
                async: false,
                success: function (data) {
                    result = data.d.Title;
                },
                error: function (err) {
                    console.log("There was an error" + err);
                }
            });
    
            return result;
    
        },

        getFilterRequestUrl: function (filter = '') {

            if (!filter) {
                filter = '$filter=';
            }
            else {
                filter += ' and '
            }

            if (productCatalog.isLookingActiveAds) {
                filter += `Status/Value eq 'Active'`;
            }
            else {
                filter += `Status/Value eq 'Moderation'`;
            }

            if (productCatalog.selectedCategoryId != 0) {
                filter += ` and Category/Id eq ${productCatalog.selectedCategoryId}`;
            }

            if (productCatalog.isLookingUserAds) {
                filter += ` and CreatedById eq ${productCatalog.currentUserId}`;
            }

            return filter;
        },

            //#region Page Management

            updateCurrentPage: (param) => {
                productCatalog.currentPage = param;
                window.localStorage.setItem('currentPage', productCatalog.currentPage);
            },

            paginate: function(page, target) {

                $('.filter').removeClass('active');
                $(target).addClass('active');
    
                productCatalog.updateCurrentPage(page);
                productCatalog.displayAdvertisements(page);
            },
    
            paginateNext: function() {
    
                if (productCatalog.currentPage + 1 <= productCatalog.pageCount) {
    
                    $('.filter').removeClass('active');
    
                    $(`div.pager ul li#${productCatalog.currentPage + 1}`).addClass('active');
    
                    productCatalog.updateCurrentPage(productCatalog.currentPage += 1);
                    productCatalog.displayAdvertisements(productCatalog.currentPage);
                }
            },

            fillPager: function (itemsCount) {

                productCatalog.pageCount = Math.ceil(itemsCount / productCatalog.pageSize);

                // Clear the Pager element
                let pager = $('div.pager ul');
                pager.html('');

                // Display warning if there is no items to show
                if (productCatalog.pageCount == 0) {
                    return;
                }

                // Fill the Pager element
                for (i = 1; i <= productCatalog.pageCount; i++) {

                    let elClass = 'filter';
                    if (i == productCatalog.currentPage) {
                        elClass += ' active';
                    }

                    pager.append(
                        `<li id="${i}" class="${elClass}"><a  href="#${i}">${i}</a></li>`
                    );

                }

                pager.append(
                    `<li id="next" class="filter"><a  href="#next">Следующая</a></li>`
                );
            },

            //#endregion

            fillAdvertisements: function(items) {

                let advertisementsContainer = $('div.news-box.section-padding');
                let advertisementsHTML = '';

                console.log(items);
    
                $.each(items, function (index, item) {
    
                    let category = productCatalog.getListProperty('Categories', item.CategoryId, 'Title');
                    let userInfo = getUserInfo(item.CreatedById);
                    let userLink = getUserLink(item.CreatedById);
                    let ImageUrl = getAttachmentUrls(productCatalog.webAbsoluteUrl, item.Id, true);
    
                    let date = item.Created.replace(/\D/g, '');
                    date = getLocalDate(date * 1, true);
                    advertisementsHTML += `
                
                <div class="news-item">
                    <div class="row">
                        <div class="col-sm-4 col-xs-12">
                            <div class="img">
                            <a href><img src="${ImageUrl}" alt="${ImageUrl}"></a>
                            </div>
                        </div>
        
                        <div class="col-sm-8 col-xs-12">
                            <div class="text">
                                <div class="date date-bottom">${date}</div>
                                <div class="status">${category}</div>
                                <div class="name">
                                    <a href="${_spPageContextInfo.webAbsoluteUrl}/Pages/FullAdvertisement.aspx?$id=${item.Id}" target="_blank">${item.Title}</a>
                                </div>
                                <div class="ico-name">
                                    <a href=${userLink}>
                                        <div class="ico">
                                            <img src="${userInfo.Image}" alt>    
                                        </div>
                                        <div class="n">${userInfo.Name}</div>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`;
    
                });
    
                $('div.news-item.pending').remove();
                advertisementsContainer.prepend(advertisementsHTML);
            },

            displayAdvertisements: function (page) {

                // Clear container and set Pending status
                $('.news-item').remove();
                $('div.news-box.section-padding').prepend('<div class="news-item pending">Загрузка...</div>');

                // Get count of Items for current selected conditions
                const AdvertisementsAPIEndpoint = productCatalog.getAPIEndpoint('Advertisements')
                let lookingFields = '$select=Id,Category/Id,Status&$expand=Category';
                let filter = productCatalog.getFilterRequestUrl();

                const requestItemCountURL = `${AdvertisementsAPIEndpoint}?${lookingFields}&${filter}&$inlinecount=allpages`;

                $.ajax({
                    url: requestItemCountURL,
                    type: "GET",
                    headers: {
                        "accept": "application/json;odata=verbose"
                    },
                    async: false,
                    success: function (data) {
                        productCatalog.fillPager(data.d.__count);
                    },
                    error: function (err) {
                        console.log("There was an error" + err);
                    }
                });

                if (productCatalog.pageCount == 0) {
                    $('.news-item').remove();
                    $('div.news-box.section-padding').prepend('<div class="news-item pending">Нет элементов для отображения...</div>');
                    return;
                }

                const skipToken = `&$skip=${(page - 1) * productCatalog.pageSize}&$top=${productCatalog.pageSize}`
                const requestItemsURL = `${AdvertisementsAPIEndpoint}?${filter}${skipToken}&$orderby=Created desc`;

                $.ajax({
                    url: requestItemsURL,
                    type: "GET",
                    headers: {
                        "accept": "application/json;odata=verbose"
                    },
                    success: function (data) {
                        console.log(data)
                        productCatalog.fillAdvertisements(data.d);
                    },
                    error: function (err) {
                        console.log("There was an error" + err);
                    }
                });
            },

            loadAdvertisements: function () {

                productCatalog.currentPage = parseInt(window.localStorage.getItem('currentPage'));
                if (!productCatalog.currentPage) {
                    productCatalog.currentPage = 1;
                }

                productCatalog.isCurrentUserAdmin = isGroupMember(_spPageContextInfo.webAbsoluteUrl, productCatalog.currentUserId, "marketplace Owners");

                if (productCatalog.isCurrentUserAdmin) {
                    $('.inp.btns-inp').removeClass('inactive');  
                }
                else {
                    $('.inp.btns-inp').remove();
                }

                productCatalog.displayCategories();
                productCatalog.displayAdvertisements(productCatalog.currentPage);

            },
            }

    productCatalog.loadAdvertisements();


    //#region Events

    // onclick button Status
    $('.btns').on('click', '.btn', function() {
       
        let item = $(this)[0];
        let isChanged = productCatalog.isLookingActiveAds;

        if (item.innerHTML == 'На модерации' && productCatalog.isLookingActiveAds) {
            $('#second')[0].innerHTML = 'Активные';
            $('#first')[0].innerHTML = 'На модерации';
            productCatalog.isLookingActiveAds = false;
        }   
        else if (item.innerHTML == 'Активные' && !productCatalog.isLookingActiveAds) {
            $('#first')[0].innerHTML = 'Активные';
            $('#second')[0].innerHTML = 'На модерации';
            productCatalog.isLookingActiveAds = true;
        }

        isChanged = isChanged == productCatalog.isLookingActiveAds ? false : true;

        if (isChanged) {
            productCatalog.updateCurrentPage(1)
            productCatalog.displayAdvertisements(productCatalog.currentPage);
        }

    });

    // onchange Selected Category
    $('.inp.select-inp').on('change', '#Categories', function(){

        let selectedValue = $("Select#Categories option:selected").val();
        productCatalog.selectedCategoryId = selectedValue;
        productCatalog.displayAdvertisements(1);

    });

    // onclick User Ads
    $('.ch').on('change', '#c1', function(){

        let item = $(this)[0];

        if ($(item).is(":checked")) {
            productCatalog.isLookingUserAds = true;
        }
        else {
            productCatalog.isLookingUserAds = false;
        }

        productCatalog.updateCurrentPage(1)
        productCatalog.displayAdvertisements(productCatalog.currentPage);

    });

    // onclick Create Advertisement Btn
    $('.thanks-from.default-form.cat-form.desk-form').on('click', 'input.btn', function(){
        window.location.href = `${productCatalog.webAbsoluteUrl}/Pages/EditAdvertisement.aspx`;
    });

    // onclick pager item
    $('div.pager ul').on('click', 'li', function(){
        
        let item = $(this)[0]

        if(item.id != 'next'){
            productCatalog.paginate(item.id, item);
        }
        else{
            productCatalog.paginateNext();
        }

    })

    //#endregion
};

$(document).ready(function () {

    if ($("#MSOLayout_InDesignMode").attr('value') != "1") {
        updatePageProperties(document, 'Объявления');
        SP.SOD.executeFunc("sp.js", "SP.ClientContext", executeProductCatalog)
    }

});