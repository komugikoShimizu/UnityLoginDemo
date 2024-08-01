<?php
$servername = "localhost";
$username = "root";
$password = "sumito0107";
$dbname = "testDatabase";

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$name = $_POST['name'];
$state = $_POST['state'];
$text = $_POST['text'];

$sql = "INSERT INTO userData (name, state, text) VALUES ('$name', $state, '$text');";
// echo $sql;
if ($conn->query($sql) === TRUE) {
    $last_id = $conn->insert_id;
    echo $last_id;
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();
?>
