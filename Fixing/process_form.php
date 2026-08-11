<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $name = $_POST["name"];
    $email = $_POST["email"];
    $message = $_POST["message"];

    $to = "kossur@example.com"; // Replace with your email address
    $subject = "Repository Form Submission";

    $headers = "From: $email\r\n";
    $headers .= "Reply-To: $email\r\n";
    $headers .= "Content-type: text/html\r\n";

    $mailBody = "Name: $name<br>Email: $email<br>Message:<br>$message";

    // Use mail() function to send the email
    mail($to, $subject, $mailBody, $headers);

    // Redirect to a thank you page or display a success message
    // header("Location: thank_you.html");
    exit();
}
?>
