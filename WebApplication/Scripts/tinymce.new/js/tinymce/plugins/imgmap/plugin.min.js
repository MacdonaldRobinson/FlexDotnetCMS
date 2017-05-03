(function() {
	tinymce.PluginManager.requireLangPack('imgmap', 'en,fr');

    tinyMCE.addI18n('en.imgmap', {
        title : 'Image Map Editor',
        desc : 'Image Map Editor',
        remove : 'Remove map'
    });

	
	tinymce.PluginManager.add('imgmap', function(ed, url) {
        // Register commands
        ed.addCommand('mceimgmapPopup', function() {
            var e = ed.selection.getNode();

            // Internal image object like a flash placeholder
            if (ed.dom.getAttrib(e, 'class').indexOf('mceItem') != -1)
                return;

            ed.windowManager.open({
                file : url + '/popup.html',
                width : 700,
                height : 560
            }, {
                plugin_url : url
            });
        });

        // Register buttons
        //tinyMCE.getButtonHTML(cn, 'lang_imgmap_desc', '{$pluginurl}/images/tinymce_button.gif', 'mceimgmapPopup');
        ed.addButton('imgmap', {
            title : 'imgmap.desc',
            cmd : 'mceimgmapPopup',
            image : url + '/images/tinymce_button.gif',
            onPostRender: function() {
                var ctrl = this;

                ed.on('NodeChange', function (event) {
                    var node = event.element;

                    if (node == null)
                        return;

                    //check parents
                    //if image parent already has imagemap, toggle selected state, if simple image, use normal state
                    do {
                        //console.log(node.nodeName);
                        if (node.nodeName == "IMG" && ed.dom.getAttrib(node, 'class').indexOf('mceItem') == -1) {
                            if (ed.dom.getAttrib(node, 'usemap') != '') {
                                ctrl.disabled(false);
                                ctrl.active(true);
                            }
                            else {
                                ctrl.disabled(false);
                                ctrl.active(false);
                            }
                            return true;
                        }
                    }
                    while ((node = node.parentNode));

                    //button disabled by default
                    ctrl.disabled(true);
                    ctrl.active(false);
                    return true;
                });
            },
        });

    });

    /* Removed since 4?
    getInfo : function() {
        return {
            longname  : 'Image Map Editor',
            author    : 'Gagaro, Adam Maschek, John Ericksen',
            authorurl : 'https://github.com/Gagaro/tinymce-imgmap',
            infourl   : 'https://github.com/Gagaro/tinymce-imgmap',
            version   : "4.0beta1"
        };
    }*/
})();