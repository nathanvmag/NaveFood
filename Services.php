
<?php
    $servername = "localhost";
	$username = "u120627928_nat";
	$pass = "24842288";
	$dbname ="u120627928_nfood";
    $servID="";

    $connect = new mysqli($servername,$username,$pass,$dbname);
    if (isset($_POST['servID']))
    {
        $servID= $_POST['servID'];
    }
    
	if ($conn->connect_error) {
                die("Connection failed: " . $conn->connect_error);
	                           } 
                               
	else {
    if ($servID==21){
    if(isset( $_POST['Produto']) && isset( $_POST['img'])&& isset( $_POST['vend']) && isset( $_POST['turma']) && isset( $_POST['preco']) && isset ($_POST['disp'])){
        $produto =  $_POST['Produto'];
        $img = $_POST['img'];
        $vendedor = $_POST['vend'];
        $wpp = $_POST['wpp'];
        $turma= $_POST['turma'];
        $preco = $_POST['preco'];
        $disp= $_POST['disp'];
        $fbid=$_POST['fbid'];
	    $query1 = "INSERT INTO `NaveFood`(`Image`, `Produto`, `Vendedor`, `Whatsapp`, `Turma`, `Preco`,`fbID`, `Dispo`) VALUES ('$img','$produto','$vendedor','$wpp','$turma','$preco','$fbid',$disp)";
        $connect->query($query1);
        $query = "SELECT MAX( id ) AS lastid
            FROM NaveFood";
       $result = $connect->query($query);
       if ($result->num_rows > 0) 
       {
	    while ($row = $result->fetch_assoc()) {
        echo $row['lastid'];
                 }
	    }
	    else echo "sem resultados /n";
             }

        else echo "error";
        }
        else if ($servID==212)
        {
        
            if (isset($_POST['id']))
            {
                $id=$_POST['id'];

                $query1 = "DELETE FROM `NaveFood` WHERE id='$id'";
                $connect->query($query1);
               echo "deleted";
            }      
        }
       else if ($servID==315){
            if (isset($_POST['id']) && isset($_POST['Produto']) && isset( $_POST['img'])&& isset( $_POST['vend']) && isset( $_POST['turma']) && isset( $_POST['preco'])&& isset ($_POST['disp']))
            {
                $id = $_POST['id'];
                $produto =  $_POST['Produto'];
                $img = $_POST['img'];
                $vendedor = $_POST['vend'];
                $wpp = $_POST['wpp'];
                $turma= $_POST['turma'];
                $preco = $_POST['preco'];
                $disp= $_POST['disp'];
                $fbid=$_POST['fbid'];
                $query1 = "UPDATE `NaveFood` SET `Image`='$img',`Produto`='$produto',`Vendedor`='$vendedor',`Whatsapp`='$wpp',`Turma`='$turma',`Preco`='$preco',`fbID`='$fbid',`Dispo`='$disp' WHERE id ='$id'";
                $connect->query($query1);
                echo"sucess";
            }
            else echo "erro";
        
        }
        // NOVOS SERVIÇOS 
         else if ($servID ==31)
        {
        
             if (isset($_POST['login']) && isset($_POST['senha'])){
             $login = $_POST['login'];
             $senha= $_POST['senha'];
              $senha = md5($senha);
             $query = "INSERT INTO `UserLogin`( `Login`, `Senha`) VALUES ('$login','$senha')";
	        $result = $connect->query($query);
            echo "ok";
            }
            else echo "error";
            
        }
         else if ($servID ==49)
        {
        
             if (isset($_POST['login']) ){
             $login = $_POST['login'];             
             $query = "SELECT `Login` as 'login' FROM `UserLogin` WHERE `Login`='$login' ";
	         $result = $connect->query($query);

             if ($result->num_rows > 0) {
	        while ($row = $result->fetch_assoc()) {
                echo "error";
               }
            }
             else echo "Ok";
              
        }
           else  echo "error";
}

        
           else if ($servID ==37)
        {
        
             if (isset($_POST['login']) && isset($_POST['senha'])){
             $login = $_POST['login'];
             $senha= $_POST['senha'];
             $senha = md5($senha);
             $query = "SELECT `Login` as 'login', `Senha`as 'pass'  FROM `UserLogin` WHERE `Login`='$login' and `Senha`='$senha'";
	         $result = $connect->query($query);

             if ($result->num_rows > 0) {
	        while ($row = $result->fetch_assoc()) {
                echo "Ok";
               }
            }
             else echo "errorsemresult";
              
        }
           else  echo "error";
       }
       else if ($servID ==65)
        {
         if (isset($_POST['login']) ){
             $login = $_POST['login'];      
            $query = "SELECT  `Nome`as 'name', `foto`as 'foto', `Turma`as 'turma', `cel` as 'cel',`fbID`  as 'fb' FROM `UserLogin` WHERE `Login`= '$login'";
	        $result = $connect->query($query);

	        if ($result->num_rows > 0) {
	        while ($row = $result->fetch_assoc()) {
                echo $row["name"]. "|". $row["turma"]. "|". $row["cel"]. "|". $row["foto"]."|".$row["fb"] ;
               }
	                                     }
	        else echo "sem resultados /n";
         }
         else echo "error";
         }
         else if ($servID=92)
         {
             if (isset($_POST['login'])&&isset($_POST['nome'])&&isset($_POST['foto'])&&isset($_POST['turma'])&&isset($_POST['cel'])&&isset($_POST['fbid']) ){
             $login = $_POST['login'];      
             $nome= $_POST['nome'];
             $foto= $_POST['foto'];
             $turma=$_POST['turma'];
             $cel=$_POST['cel'];
             $fbid=$_POST['fbid'];
            $query = "UPDATE `UserLogin` SET `Nome`='$nome',`foto`='$foto',`Turma`='$turma',`cel`='$cel',`fbID`='$fbid' WHERE `Login`= '$login'";
	        $result = $connect->query($query);
            echo "OK";
            }
            else echo "error";

            }
           
       
    
//TERMINA AQUI

        else if ($servID ==920)
        {
            $query = "SELECT  `Image` as 'img', `Produto`as 'prod', `Vendedor`as 'vend', `Whatsapp`as 'wpp', `Turma`as 'turma', `Preco`as 'preco', `fbID` as 'fbid', `Dispo`as 'disp'  FROM `NaveFood` WHERE 1";
	        $result = $connect->query($query);

	        if ($result->num_rows > 0) {
	        while ($row = $result->fetch_assoc()) {
                echo $row["img"]. "|". $row["prod"]. "|". $row["vend"]. "|". $row["wpp"]. "|". $row["turma"]. "|". $row["preco"]. "|". $row["fbid"]. "|". $row["disp"].";" ;
               }
	                                     }
	        else echo "sem resultados /n";
         }
            else echo "noSERVID";
        }
        
?>	
