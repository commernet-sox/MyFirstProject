window.AudioContext = window.AudioContext || window.webkitAudioContext || window.mozAudioContext || window.msAudioContext;
try {
    var context = new window.AudioContext();
    var source = null;
    var audioBuffer = null;
    function stopSound() {
        if (source) {
            source.stop(0); //立即停止
        }
    }
    function playSound() {
        source = context.createBufferSource();
        source.buffer = audioBuffer;
        source.loop = false;
        source.connect(context.destination);
        source.start(0); //立即播放
    }
    function initSound(arrayBuffer) {
        context.decodeAudioData(arrayBuffer, function (buffer) { //解码成功时的回调函数
            audioBuffer = buffer;
            playSound();
        }, function (e) { //解码出错时的回调函数
            console.log('Error decoding file', e);
        });
    }
    function loadAudioFile(url) {
        var xhr = new XMLHttpRequest(); //通过XHR下载音频文件
        xhr.open('GET', url, true);
        xhr.responseType = 'arraybuffer';
        xhr.onload = function (e) { //下载完成
            initSound(this.response);
        };
        xhr.send();
    }
} catch (e) {
    alert('!您的浏览器不支持 AudioContext');
}

function playOk() {
    loadAudioFile("/media/ok.wav");
}

function playSuccess() {
    loadAudioFile("/media/success.mp3");
}

function playNotExist() {
    loadAudioFile("/media/notexist.mp3");
}

function playError() {
    loadAudioFile("/media/error.mp3");
}

