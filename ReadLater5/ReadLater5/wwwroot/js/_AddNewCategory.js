var addNewCategory = function () {

    var self = this;

    function init() {

        function _initDom() {
            self._dom = {};

            //btns
            self._dom.showAddCategory = $("#add-new-category-btn");
            self._dom.backToCategorySelection = $("#back-to-select-category-btn");
            self._dom.addCategory = $("#add-category-btn");

            //divs
            self._dom.newCategorySection = $("#add-new-category-section");
            self._dom.existingCategorySection = $("#select-existing-category-section");

            self._dom.categoryNameLabel = $("#Category_Name");

            self._dom.categoryDropdown = $("#CategoryId");
        }

        function _bindEvents() {
            self._dom.showAddCategory.on("click", _onClick);
            self._dom.backToCategorySelection.on("click", _onClick);
            self._dom.addCategory.on("click", _onClickAddCategory);
        }

        _initDom();
        _bindEvents();
    }

    function _onClick() {
        if (self._dom.existingCategorySection.is(":visible")) {
            self._dom.newCategorySection.attr("hidden", false);
            self._dom.existingCategorySection.hide();
        }
        else {
            self._dom.existingCategorySection.show();
            self._dom.newCategorySection.attr("hidden", true);
        }
    }

    function _onClickAddCategory() {
        var url = self._dom.addCategory.data("url");
        var categoryName = self._dom.categoryNameLabel.val();

        $.ajax({
            url: url,
            type: "POST",
            dataType: "json",
            data: { categoryName: categoryName },
            success: function (data) {

                console.log(data);
                console.log(data.id);
                console.log(data.name);

                _onClick();
                self._dom.categoryDropdown.append($('<option>', {
                    value: data.id,
                    text: data.name
                }));
                self._dom.categoryNameLabel.val("");
            }            
        });
    }

    return {
        init: init
    }
}();

$(function () {
    addNewCategory.init();
});


