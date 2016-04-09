$(document).ready(function(){	
	

    $('input[name=radio-sort]').click(function(){
        var li=$(this).data('item');
		$('.filters label').removeClass("btn-info");
		$('#'+li).addClass("btn-info");
        if(li=='ff-item-type-all'){ 
			$('.thumbnails li').fadeTo(500,1); 
		}else{
            if($(this).is(':checked')){
                $('.thumbnails li').not('.'+li).fadeTo(500,0.1);
                $('.thumbnails li.'+li).fadeTo(500,1);
            }
        }
    });

	$('#full-img').click(function(){$('#previewLightbox').lightbox('hide'); });
	$('.upload-btn').click(function(){
		$('.uploader').show(500);
	});
	$('.close-uploader').click(function(){
		$('.uploader').hide(500);
		window.location = removeVariableFromURL(window.location, 'cmd');
	});
	$('.preview').click(function(){
		$('#full-img').attr('src',$(this).data('url'));
		return true;
	});
	$('.new-folder').click(function(){
	    folder_name = window.prompt('Insert folder name:', 'New Folder');
		if(folder_name){
		folder_path=curr_dir + '\\' + folder_name;
		$.ajax({
			  type: "POST",
			  url: "dialog.aspx?cmd=createfolder",
			  data: {folder: folder_path}
	    }).done(function (msg) {
	        //TODO: add error handling
	        window.location = removeVariableFromURL(window.location, 'cmd');
		});
		}
	});

	var boxes = $('.boxes');
	boxes.height('auto');
	var maxHeight = Math.max.apply(
	  Math, boxes.map(function() {
	    return $(this).height();
	}).get());
	boxes.height(maxHeight);
});

function apply(file, type_file) {
    var target = window.parent.document.getElementById(track+'_ifr');
    //var closed = window.parent.document.getElementsByClassName('mce-tinyfilemanager.net');
    var closed = window.parent.document.getElementsByClassName('mce-tinyfilemanager.net');
    var ext=file.split('.').pop();
    var fill='';
    if($.inArray(ext.toLowerCase(), ext_img) > -1){
        fill=$("<img />",{"src":file});
    }else{
        fill=$("<a />").attr("href", file).text(file.replace(/\..+$/, ''));
    }
    $(target).contents().find('#tinymce').append(fill);
    $(closed).find('.mce-close').trigger('click');
}

function apply_link(file,type_file){
	$('.mce-link_'+track, window.parent.document).val(file);
	//var closed = window.parent.document.getElementsByClassName('mce-tinyfilemanager.net');
	var closed = window.parent.document.getElementsByClassName('mce-tinyfilemanager.net');
	if($('.mce-text_'+track, window.parent.document).val()=='') $('.mce-text_'+track, window.parent.document).val(file.replace(/\..+$/, ''));
    $(closed).find('.mce-close').trigger('click');
}

function apply_img(file,type_file){
    //var target = window.parent.document.getElementsByClassName('mce-img_'+track);
    var target = window.parent.document.getElementsByClassName('mce-img_' + track);
    //var closed = window.parent.document.getElementsByClassName('mce-tinyfilemanager.net');
    var closed = window.parent.document.getElementsByClassName('mce-tinyfilemanager.net');
    $(target).val(file);
    $(closed).find('.mce-close').trigger('click');
}

function apply_video(file,type_file){
    //var target = window.parent.document.getElementsByClassName('mce-video'+ type_file +'_'+track);
    var target = window.parent.document.getElementsByClassName('mce-video' + type_file + '_' + track);
    //var closed = window.parent.document.getElementsByClassName('mce-tinyfilemanager.net');
    var closed = window.parent.document.getElementsByClassName('mce-tinyfilemanager.net');
    $(target).val(file);
    $(closed).find('.mce-close').trigger('click');
}

function removeVariableFromURL(url_string, variable_name) {
    var URL = String(url_string);
    var regex = new RegExp("\\?" + variable_name + "=[^&]*&?", "gi");
    URL = URL.replace(regex, '?');
    regex = new RegExp("\\&" + variable_name + "=[^&]*&?", "gi");
    URL = URL.replace(regex, '&');
    URL = URL.replace(/(\?|&)$/, '');
    regex = null;
    return URL;
}
