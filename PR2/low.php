<?php

if( isset( $_GET[ 'Submit' ] ) ) {
    $id = $_GET[ 'id' ];

	$getid  = "SELECT first_name, last_name FROM users WHERE user_id = :user_id";
	$stmt = $pdo->prepare($getid);
	$stmt->bindParam(":user_id", $id);
	$stmt->execute();
	
	$hasId = false;
    	if ( $stmt->rowCount() == 1 ) {
      		$hasId = true;
    	
		echo '<pre>User ID exists in the database.</pre>';
	}
	else {
		header( $_SERVER[ 'SERVER_PROTOCOL' ] . ' 404 Not Found' );
		echo '<pre>User ID is MISSING from the database.</pre>';
	}
}

?>
