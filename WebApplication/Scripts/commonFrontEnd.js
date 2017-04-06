$(document).ready(function () {

    if ($("form").length > 0)
    {
        $("input:submit").click(function (event) {

            var mainForm = $("form")[0];
            var dataForm = $(this).parents("div[data-form]")[0];

            var hasTarget = false;

            if (dataForm != null) {
                for (var i = 0; i < dataForm.attributes.length; i++) {
                    var attrib = dataForm.attributes[i];

                    if (attrib.name == "target")
                        hasTarget = true;

                    $(mainForm).attr(attrib.name, attrib.value);
                }
            }

            mainForm.submit();

            if (hasTarget) {
                window.reload();
            }
        });
    }
  
    $.get(BaseUrl + "WebServices/IMediaDetails.asmx/GetGlossaryTerms", function (data) {
        $(data).each(function () {
            var term = this.Term;
            var definition = this.Definition;

            var regex = new RegExp("\\b" + term + "\\b(?![^<]*</[a|span]+>)", "gi");

            var replacedTerm = [];

            $("p:contains(" + term + "), li:contains(" + term + ")").not(".cd-nav li").each(function () {
                var html = $(this).html();

                if ($(this).find("span[title~='" + term + "']").length > 0)
                    return false;

                var html = html.replace(regex, function (match, offset, original) {
                    return "<span data-toggle='tooltip' title='" + definition + "'>" + match + "</span>";
                });

                $(this).html(html);
            });
        });
    });

});