# Youpload
A simple tool to upload screenshots based on ShareX library, available here at http://youpload.cornat.co/

## Features

* Capture Area
* Capture FullScreen
* Upload Clipboard
* Upload File
* Hotkeys

Configure in YouploadSettings.cs the 'UploadUrl' string.
The server must anwser in json format, ex :

{"status":200,"data":"http://youpload.cornat.co/public/422ff8b0.png"}

```
status
    200 : Ok    
    500 : Error      
data
    Image url
```

## Screenshots

Contextual Menu

![20200419-004754](https://user-images.githubusercontent.com/20048259/79673096-92426d00-81d7-11ea-8dbc-bc61a607f89a.png)

Select Area

![20200419-004935](https://user-images.githubusercontent.com/20048259/79673104-aa19f100-81d7-11ea-974f-43a18ab7068a.png)

Notification Form

![20200419-004738](https://user-images.githubusercontent.com/20048259/79673110-b4d48600-81d7-11ea-94e7-0f4627b5e568.png)

Upload History

![20200419-005114](https://user-images.githubusercontent.com/20048259/79673134-e2b9ca80-81d7-11ea-9e3d-05334c776bef.png)

## Available here

http://youpload.cornat.co/