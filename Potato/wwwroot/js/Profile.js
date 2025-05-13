document.getElementById("fileInput").addEventListener("change", function () {
    if (this.files.length > 0) {
        document.getElementById("photoForm").submit();
    }
});