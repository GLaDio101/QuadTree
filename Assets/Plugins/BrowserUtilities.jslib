mergeInto(LibraryManager.library, {

    CurrentHost: function () {
        var bufferSize = lengthBytesUTF8(document.location.hostname) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(document.location.hostname, buffer, bufferSize);
        return buffer;
    },

    CurrentPort: function () {
        return parseInt(document.location.port);
    },

    IsSsl: function () {
        return document.location.protocol == "https:";
    },
});