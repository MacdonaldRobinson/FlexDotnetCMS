$(document).ready(function () {

    $("input:submit").click(function (event) {
        var mainForm = $("form")[0];
        var dataForm = $(this).parents("div[data-form]")[0];

        var hasTarget = false;

        if (dataForm != null)
        {
            for (var i = 0; i < dataForm.attributes.length; i++) {
                var attrib = dataForm.attributes[i];

                if (attrib.name == "target")
                    hasTarget = true;

                $(mainForm).attr(attrib.name, attrib.value);
            }
        }

        mainForm.submit();

        if (hasTarget)
        {
            window.reload();
        }

    });
});