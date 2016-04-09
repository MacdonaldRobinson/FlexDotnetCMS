<?php
// SET YOUR FACEBOOK API DETAILS HERE
$app_id 	= 'INSERT APP ID HERE';
$app_secret = 'INSERT APP SECRET HERE';

// DO NOT EDIT BELOW THIS LINE
ini_set('display_errors', '0');
error_reporting(E_ALL | E_STRICT);

$app_access_token = $app_id.'|'.$app_secret;
$page_id = isset($_GET['id']) ? $_GET['id'] : '';
$limit = isset($_GET['limit']) ? $_GET['limit'] : 20;
$feed = isset($_GET['feed']) ? $_GET['feed'] : 'feed';
$fields = "id,message,picture,link,name,description,type,icon,created_time,from,object_id,likes,comments";
$graphUrl = 'https://graph.facebook.com/v2.3/'.$page_id.'/'.$feed.'?key=value&access_token='.$app_access_token.'&fields='.$fields;

$graphObject = file_get_contents($graphUrl);

if ( $graphObject === false )
{
   $graphObject = dc_curl_get_contents($graphUrl);
}

$parsedJson  = json_decode($graphObject);
$pagefeed = $parsedJson->data;
$count = 0;
$link_url = '';
$json_decoded = array();

$json_decoded['responseData']['feed']['link'] = "";
if(is_array($pagefeed)) {

	foreach($pagefeed as $data)
	{
		
		if(isset($data->message))
		{
			$message = $data->message;
		} else if(isset($data->story))
		{
			$message = $data->story;
		} else {
			$message = '';
		}
		
		if(isset($data->description))
		{
			$message .= ' ' . $data->description;
		}
		
		$link = isset($data->link) ? $data->link : '';
		$image = isset($data->picture) ? $data->picture : null;
		$type = isset($data->type) ? $data->type : '';
		
		if($link_url == $link){
			continue;
		}
		
		$link_url = $link;
		
		if($message == '' || $link == '') {
		//	continue;
		}
		
		if($type == 'status' && isset($data->story)) {
			continue;
		}
		
	//	if($type == 'event') {
	//		$link = 'https://facebook.com' . $link;
	//	}

		if(!isset($data->object_id) && $type != 'video') {
			$pic_id = explode("_", $image);	
			if(isset($pic_id[1])){
				$data->object_id = $pic_id[1];
			}
		}
		
		if(isset($data->object_id)){
		
			if(strpos($image, 'safe_image.php') === false && is_numeric($data->object_id)) {
				$image = 'http://graph.facebook.com/'.$data->object_id.'/picture?type=normal';
			}
		
		}

		$json_decoded['responseData']['feed']['entries'][$count]['link'] = $link;
		$json_decoded['responseData']['feed']['entries'][$count]['content'] = $message;
		$json_decoded['responseData']['feed']['entries'][$count]['thumb'] = $image;
		$json_decoded['responseData']['feed']['entries'][$count]['publishedDate'] = date("D, d M Y H:i:s O", strtotime($data->created_time));
		$count++;
	}
}

function dc_curl_get_contents($url)
{
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_HEADER, 0);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($ch, CURLOPT_URL, $url);
    $data = curl_exec($ch);
    curl_close($ch);
    return $data;
}

header("Content-Type: application/json; charset=UTF-8");
echo json_encode($json_decoded);
?>