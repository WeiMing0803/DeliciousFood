$("#file-1").fileinput({
    uploadUrl: '', // 这个是点击上传时候的上传接口
    allowedFileExtensions: ['jpg', 'png', 'gif', 'jpeg'],//允许的文件类型
    overwriteInitial: false,
    language: 'zh',
    maxFileSize: 1500,//文件的最大大小 单位是k
    uploadExtraData: {  //上传的时候，增加的附加参数
        //userid: XXX,
    },
    showUpload: false, //是否显示上传按钮
    maxFilesNum: 10,//最多文件数量 
    previewSettings: {//设置预览图片时候的大小
        image: { width: "100px", height: "100px" },
    },
    slugCallback: function (filename) {
        return filename;
    }
}).on('filecleared', function () {//点击移除按钮时触发


})
    .on('fileuploaded', function (event, data, previewId, index) {//文件上传完成后触发
        console.log(event, data, previewId, index);

    });