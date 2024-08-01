<?php
// データベース接続の設定
$servername = "localhost";
$username = "root";
$password = "sumito0107";
$dbname = "testDatabase";

// 指定されたIDを取得
$id = $_GET['id']; // URLパラメータからIDを取得

// データベース接続の作成
$conn = new mysqli($servername, $username, $password, $dbname);

// 接続の確認
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// SQLクエリの作成
$sql = "SELECT * FROM userData WHERE id = $id";
$result = $conn->query($sql);

// 結果の取得と表示
if ($result->num_rows > 0) {
    // 結果が存在する場合
    while($row = $result->fetch_assoc()) {
        echo "$id:{$row["name"]}:{$row["state"]}:{$row["text"]}";
    }
} else {
    echo "0 results";
}

// 接続の終了
$conn->close();
?>
