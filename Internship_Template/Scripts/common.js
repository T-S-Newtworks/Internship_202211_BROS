function Clear() {

    //ダミー(表示用)のフォームクリア
    $('#file_name').val("");
    //本来のフォームクリア
    $('#file').val("");
}
/**
 *ファイル入力フォームに選択したファイル情報を入れる。また、アップロードした画像を表示する。
 **/
$(function () {
    $(document).on('change', 'input[type="file"]', function () {
        //本来の入力フォームのID名称を取得
        var file = $(this).prop('files')[0];
        $('#file_name').val(file.name);

        //画像ファイル以外は表示しない。
        if (!file.type.startsWith("image/"))
            return;

        //ファイルリーダーを呼び出してアップロードファイルを読み込み、画像の部分に置き換える。
        var reader = new FileReader();
        reader.onloadend = function () {
            var img = $('#userIcon')[0];
            img.src = reader.result;
        }
        reader.readAsDataURL(file);
    });
    //TODO: ファイルサイズチェックぐらいはするべき
    //    if (8196 * 1024 < uploaded_file.size) {
    //        document.getElementById('dispchange').style.display = "inline-block";
    //    }
});
