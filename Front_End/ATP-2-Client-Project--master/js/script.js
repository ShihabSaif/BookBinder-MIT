// //

$(function () {
	$(".LogInContainer").show();

	var credentials = '';
	var email = "", password = "", id = 0, admin = false;

	var hideAndSeek = function () {
		$(".BookDetails").hide();
		$(".uniqueCategory").hide();
		$(".LogInContainer").hide();
		$(".registrationContainer").hide();
		$(".editContainer").hide();
		$(".createContainer").hide();
		$(".editUserContainer").hide();
		$(".UserProfileContainer").hide();
		$(".userDetails").hide();
		$(".newsfeed").hide();
	}

	var editBook = function (e) {
		hideAndSeek();
		$(".editContainer").show();
		var id = e.target.getAttribute('data-id');
		//$( "em" ).attr( "title" );
		//alert(id);
		$.ajax({
			url: 'http://localhost:50477//api//Book/' + parseInt(id),
			method: 'GET',
			header: 'Content-Type: application/json',
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var bookList = xmlhttp.responseJSON;
					$("#editTitle").val(bookList.title);
					$("#editAuthor").val(bookList.author);
					$("#editCategory").val(bookList.category);
					$("#editDescription").val(bookList.description);
					$("#editId").val(id);
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	};

	var deleteBook = function (e) {
		var id = e.target.getAttribute('data-id');
		//$( "em" ).attr( "title" );
		//alert(id);
		$.ajax({
			url: 'http://localhost:50477//api//Book/' + parseInt(id),
			method: 'DELETE',
			headers: {
				"Authorization": "Basic " + btoa(credentials)
			},
			complete: function (xmlhttp) {
				if (xmlhttp.status == 204) {
					alert("Delete Successful");
					showBookFun();
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	};

	var categoryShow = function (e) {
		hideAndSeek();
		$(".BookDetails").show();
		$("#tblBody tr").remove();
		var name = e.target.getAttribute('data-cat');
		$.ajax({
			url: 'http://localhost:50477//api//Book/category/' + name,
			method: 'GET',
			header: 'Content-Type: application/json',
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var bookList = xmlhttp.responseJSON;
					populateBook(bookList);
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	};

	$('#btnCategory').click(function () {
		hideAndSeek();
		$(".uniqueCategory").show();

		$.ajax({
			url: 'http://localhost:50477//api//Book/uniqueCategory',
			method: 'GET',
			header: 'Content-Type: application/json',
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var bookList = xmlhttp.responseJSON;
					//alert(bookList[2]);
					var outputStr = '';
					for (var i = 0; i < bookList.length; i++) {
						outputStr += '<tr><td scope="row" class="listByCategory" data-cat="' + bookList[i] + '">' + bookList[i] + '</td></tr>';

						// onclick="editBook(' + bookList[i].BookId  + ')"
					}
					// console.log("outputStr");
					$('#categoryBody').html(outputStr);
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	});

	$('#categoryBody').on("click", "tr .listByCategory", categoryShow);

	var showBookFun = function () {
		hideAndSeek();
		$.ajax({
			url: 'http://localhost:50477//api//Book',
			method: 'GET',
			// header: 'Content-Type: application/json',
			// headers: {
			// 	// Authorization: 'Basic ' + btoa(email+':'+password)
			// 	Authorization: 'Basic ' + btoa(email+":"+password)
			// },
			contentType: "application/json",
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var bookList = xmlhttp.responseJSON;
					populateBook(bookList);
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	};

	$("uploadFile").click(function(){
		alert("OK");
	});

	$("#ratingChanges").click(function () {
		getUserAndBookId($("#modalId").val());
	});

	var updateUserBook = function (ubid) {
		$.ajax({
			url: 'http://localhost:50477//api//UserBook/' + ubid,
			method: 'PUT',
			data: {
				"rating": parseInt($("#review").val()),
				"readStatus": parseInt($("#readStatus").val())
			},
			headers: {
				"Authorization": "Basic " + btoa(credentials)
			},
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					//(parseInt($("#review").val())+" "+parseInt($("#readStatus").val()));
					alert("updated successfully");
				}
				else {
					alert(xmlhttp.statusText);
				}
			}
		});
	};

	var createUserBook = function (bookid) {
		$.ajax({
			url: 'http://localhost:50477//api//UserBook',
			method: 'POST',
			data: {
				"rating": parseInt($("#review").val()),
				"readStatus": parseInt($("#readStatus").val()),
				"userId": id,
				"bookId": bookid
			},
			headers: {
				"Authorization": "Basic " + btoa(credentials)
			},
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					alert("created successfully");
				}
				else {
					alert(xmlhttp.statusText);
				}
			}
		});
	}

	var getUserAndBookId = function (bookid) {
		$.ajax({
			url: 'http://localhost:50477//api//UserBook/UserAndBookId/' + id + '/' + bookid,
			method: 'GET',
			headers: {
				"Authorization": "Basic " + btoa(credentials)
			},
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					updateUserBook(xmlhttp.responseJSON.userBookId);
				}
				else if (xmlhttp.status == 204) {
					createUserBook(bookid);
				}
			}
		});
	}

	var getUserBookSettings = function (bookid) {
		$.ajax({
			url: 'http://localhost:50477//api//UserBook/UserAndBookId/' + id + '/' + bookid,
			method: 'GET',
			headers: {
				"Authorization": "Basic " + btoa(credentials)
			},
			// data: {
			// 	"uid": id,
			// 	"bid": bookid
			// },
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var book = xmlhttp.responseJSON;
					//alert(book.readStatus);
					$("#review").val(book.rating).change();
					$("#readStatus").val(book.readStatus).change();
					$("#exampleModal").modal('show');
				}
				else {
					$("#review").val("").change();
					$("#readStatus").val("").change();
					$("#exampleModal").modal('show');
					//alert(xmlhttp.statusText);
				}
			}
		});
	}

	var prepareModal = function (bookid) {
		// $.ajax({
		// 	url: 'http://localhost:5507//api//UserBook/AverageRating/'+bookid,
		// 	method: 'GET',
		// 	headers: {
		// 		"Authorization": "Basic " + btoa(credentials)
		// 	},
		// 	complete: function (xmlhttp) {
		// 		if (xmlhttp.status == 200) {
		// 			alert(xmlhttp.responseJSON);
		// 			//$("#modalReview").text(xmlhttp.responseJSON);
		// 		}
		// 		else{
		// 			alert(xmlhttp.responseJSON);
		// 		}
		// 	}
		// });
		$.ajax({
			url: 'http://localhost:50477//api//Book/' + bookid,
			method: 'GET',
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var book = xmlhttp.responseJSON;
					//alert(book.review);
					$(".modalTitle").text(book.title);
					$("#modalAuthor").text(book.author);
					$("#modalCategory").text(book.category);
					$("#modalReview").text(book.review);
					$("#modalDescription").text(book.description);
					$("#modalId").val(book.bookId);
					getUserBookSettings(bookid);
					// $("#exampleModal").modal('show');
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	};

	var bookRating = function (e) {
		if (id == 0) {
			alert("Log in first");
			hideAndSeek();
			$(".LogInContainer").show();
		}
		else {
			//var userid = $(".rateTheBook").attr('data-id');
			var bookid = e.target.getAttribute('data-id');
			//var userid = $(".rateTheBook").getAttribute("data-id");
			//alert(userid);
			//$("#exampleModal").modal('show');
			prepareModal(bookid);
		}
	};

	$('#showBook').click(showBookFun);

	$('#tblBody').on("click", "tr .editEachBook", editBook);
	$('#tblBody').on("click", "tr .deleteEachBook", deleteBook);
	$('#tblBody').on("click", "tr .rateTheBook", bookRating);
	$('#profileTable').on("click", "tr .rateTheBook", bookRating);

	$('#createBook').click(function () {
		$.ajax({
			url: 'http://localhost:50477//api//Book',
			method: 'POST',
			headers: {
				"Authorization": "Basic " + btoa(credentials)
			},
			data: {
				"title": $("#txtTitle").val(),
				"author": $("#txtAuthor").val(),
				"category": $("#txtCategory").val(),
				"description": $("#txtDescription").val()
			},
			complete: function (xmlhttp) {
				if (xmlhttp.status == 201) {
					showBookFun();
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	});

	$('#editBook').click(function () {
		var id = $("#editId").val();

		$.ajax({
			url: 'http://localhost:50477//api//Book/' + parseInt(id),
			method: 'PUT',
			// headers: {
			// 	// Authorization: 'Basic ' + btoa(email+':'+password)
			// 	Authorization: "Basic " + "ZEBnbWFpbC5jb206YQ=="
			// },
			headers: {
				// "Authorization": "Basic " + btoa("y@gmail.com:y")
				"Authorization": "Basic " + btoa(credentials)
			},
			// contentType: "application/json",
			data: {
				"title": $("#editTitle").val(),
				"author": $("#editAuthor").val(),
				"category": $("#editCategory").val(),
				"description": $("#editDescription").val()
			},
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					$("#editTitle").val("");
					$("#editAuthor").val("");
					$("#editCategory").val("");
					$("#editDescription").val("");
					$("#editId").val("");
					showBookFun();
				}
				else {
					alert(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	});

	var populateBook = function (bookList) {
		var outputStr = '';
		if (!admin) {
			for (var i = 0; i < bookList.length; i++) {
				outputStr += '<tr>' + '<td><img src="C:/Users/A.B.M.SHIHAB UDDIN/Desktop/download.jpg" alt="" class="img-thumbnail" style="max-width:30%;margin-right:0px"></td>' + '<td scope="row">' + bookList[i].title + '</td><td>' + bookList[i].author + '</td><td>' + bookList[i].category + '</td><td>' + (bookList[i].review).toFixed(2) + '</td><td colspan="3">' + bookList[i].description + '</td><td><button class="btn btn-primary rateTheBook" data-id="' + bookList[i].bookId + '">Rating</button></td></tr>';
			}
		}
		else {
			for (var i = 0; i < bookList.length; i++) {
				outputStr += '<tr><td scope="row">' + bookList[i].title + '</td><td>' + bookList[i].author + '</td><td>' + bookList[i].category + '</td><td>' + (bookList[i].review).toFixed(2) + '</td><td colspan="3">' + bookList[i].description + '</td><td><button class="btn btn-primary editEachBook" data-id="' + bookList[i].bookId + '">Edit</button><button class="btn btn-danger deleteEachBook" data-id="' + bookList[i].bookId + '">Delete</button></td></tr>';
			}
		}

		$('#tblBody').html(outputStr);
		$(".BookDetails").show();
	};

	var showSearchedBookFun = function () {
		hideAndSeek();
		var search = $("#txtSearch").val();
		//alert(search);
		$.ajax({
			url: 'http://localhost:50477//api//Book/search/' + search,
			method: 'GET',
			header: 'Content-Type: application/json',
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var bookList = xmlhttp.responseJSON;
					populateBook(bookList);
					$("#txtSearch").val("");
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	};

	$('#txtBtn').click(showSearchedBookFun);
	
	var globalUserId;
	var globalGuestUserId;

	var populateUser = function (searchedUser) {
		var outputStr = '';
		for(var i=0; i<searchedUser.length; i++)
		{
			globalGuestUserId = searchedUser[i].userId;
			outputStr += '<h1>' + searchedUser[i].email + globalUserId + '</h1>';
			outputStr += '<button class="btn btn-primary followUser" data-id="' + searchedUser[i].userId + '">Follow</button>';
			outputStr += '<button style="margin-left:30px;display: none" class="btn btn-primary requestedFollowUser" data-id="' + searchedUser[i].userId + '">Requested</button>';
			outputStr += '<button style="margin-left:30px;display: none" class="btn btn-primary unfollowUser" data-id="' + searchedUser[i].userId + '">Unfollow</button>';
		}		
		$('.show-searched-user').html(outputStr);
		$(".userDetails").show();
	};

	var populateFollowUser = function(searchedUser){

		//$('.show-searched-user').html(searchedUser ? "true" : "false");
		// $('.show-searched-user').html(searchedUser);
		// $(".userDetails").show();
		 if(searchedUser==="true")
		 {
			$(".followUser").hide();
			$(".requestedFollowUser").show();
			$(".unfollowUser").show();
		}
		else 
		{
			alert("You already followed this user");
		 }
	}

	var ajaxFollowUser = function () {
		
		$.ajax({
			url: 'http://localhost:50477//api//FollowUser/' + globalUserId + '/' + globalGuestUserId,
			method: 'POST',
			header: 'Content-Type: application/json',
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var searchedUser = xmlhttp.responseJSON;
					populateFollowUser(searchedUser);
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	}

	$('.show-searched-user').delegate('.followUser', 'click', ajaxFollowUser);

	var populateunFollowUser = function(searchedUser)
	{

	}

	var ajaxunFollowUser = function () {
		$(".requestedFollowUser").hide();
		$(".unfollowUser").hide();
		$(".followUser").show();
		$.ajax({
			url: 'http://localhost:50477//api//FollowUser/' + globalUserId + '/' + globalGuestUserId,
			method: 'DELETE',
			header: 'Content-Type: application/json',
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var searchedUser = xmlhttp.responseJSON;
					populateunFollowUser(searchedUser);
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	}

	$('.show-searched-user').delegate('.unfollowUser', 'click', ajaxunFollowUser);

	var showSearchedUser = function () {
		hideAndSeek();
		var search = $("#txtSearchUser").val();
		$.ajax({
			url: 'http://localhost:50477//api//User/search/' + search,
			method: 'GET',
			header: 'Content-Type: application/json',
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var searchedUser = xmlhttp.responseJSON;
					populateUser(searchedUser);
					$("#txtSearchUser").val("");
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	}

	$('#txtBtnSearchUser').click(showSearchedUser);

	var populateNewsFeed = function(showSearchedUser){
		var outputStr = '';
		for(var i=0; i<showSearchedUser.length; i++)
		{
			outputStr += '<p>' + showSearchedUser[i].userName + " added " + showSearchedUser[i].bookTitle + " to his showcase." + '</p>';
			//outputStr += '<button class="btn btn-primary followUser" data-id="' + searchedUser[i].userId + '">Follow</button>';
		}		
		$('.show-newsfeed').html(outputStr);
		$(".newsfeed").show();
	}

	var newsfeed = function(){
		hideAndSeek();
		var search = globalUserId;
		$.ajax({
			url: 'http://localhost:50477//api//FollowUser/' + search,
			method: 'GET',
			header: 'Content-Type: application/json',
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var searchedUser = xmlhttp.responseJSON;
					populateNewsFeed(searchedUser);
					$("#txtSearchUser").val("");
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
		var outputStr = '<h1>' + globalUserId + '</h1>';
		$('.show-newsfeed').html(outputStr);
		$(".newsfeed").show();
	}

	$('#txtNewsFeed').click(newsfeed);

	$("#logIn").click(function () {
		hideAndSeek();
		$(".LogInContainer").show();
	});

	$("#logOut").click(function () {
		globalUserId="";
		globalGuestUserId="";
		email = "";
		password = "";
		id = 0;
		credentials = "";
		admin = false;
		$("#createBookBtn").hide();
		$("#profileName").text("");
		$("#profileEmail").text("");
		$("#averageRatingUser").text("");
		$("#totalRead").text("");
		hideAndSeek();
		$("#logOut").hide();
		$("#logIn").show();
		$(".BookDetails").show();
		$("#tblBody tr").remove();
	});

	$("#registrationLink").click(function () {
		hideAndSeek();
		$(".registrationContainer").show();
	});

	$("#txtHome").click(function () {
		hideAndSeek();
		$(".BookDetails").show();
		$("#tblBody tr").remove();
	});

	$("#createBookBtn").click(function () {
		hideAndSeek();
		$(".createContainer").show();
	});
































	//////////////////        User     ///////////////////////

	$('#createUser').click(function () {
		if ($("#txtUserName").val() == "" || $("#txtEmail").val() == "" || $("#txtPassword").val() == "") {
			alert("All field must be filled.");
		}
		else {
			var data = {
				"user_name": $("#txtUserName").val(),
				"email": $("#txtEmail").val(),
				"password": $("#txtPassword").val(),
				"image_link" : $("#txtImage").val()
			};

                

			$.ajax({
				url: 'http://localhost:50477//api//User/duplicateMail',
				method: 'GET',
				header: 'Content-Type: application/json',
				data: {
					"email": $("#txtEmail").val()
				},
				complete: function (xmlhttp) {
					if (xmlhttp.status == 204) {
						NewUser(data);
					}
					else {
						alert("Email already exists!!");
					}
				}
			});
		}
	});

	var NewUser = function (data) {
		$.ajax({
			url: 'http://localhost:50477//api//User',
			method: 'POST',
			// cache: false,
			// contentType: false,
			// processData: false,
			data: data,
			complete: function (xmlhttp) {
				if (xmlhttp.status == 201) {
					alert("Successfully created");
					hideAndSeek();
					$(".LogInContainer").show();
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	};

	$('#btnPostFile').click(function () {  
		if ($('#file1').val() == '') {  
			alert('Please select file');  
			return;  
		}  

		var formData = new FormData();  
		var file = $('#file1')[0];  
		formData.append('file', file.files[0]);  
		$.ajax({  
			url: 'http://localhost:50477//api//User/upload',  
			type: 'POST',  
			data: formData,  
			contentType: false,  
			processData: false,  
			success: function (d) {  
				$('#updatePanelFile').addClass('alert-success').html('<strong>Upload Done!</strong><a href="' + d + '">Download File</a>').show();  
				$('#file1').val(null);  
			},  
			error: function () {  
				$('#updatePanelFile').addClass('alert-error').html('<strong>Failed!</strong> Please try again.').show();  
			}  
		});  
	});  

	$('#checkSubmit').click(function () {
		var data = {
			"email": $("#checkEmail").val(),
			"password": $("#checkPassword").val(),
		};

		$.ajax({
			url: 'http://localhost:50477//api//User/logincheck',
			method: 'GET',
			header: 'Content-Type: application/json',
			data: data,
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var data = xmlhttp.responseJSON;
					email = data.email;
					password = data.password;
					if (data.email == 'admin@gmail.com' && data.password == '123') {
						admin = true;
						$("#createBookBtn").show();
					}
					id = data.userId;
					globalUserId = data.userId;
					credentials = email + ":" + password;
					$("#checkEmail").val(""),
						$("#checkPassword").val(""),
						alert("successful!");
					hideAndSeek();
					$("#logIn").hide();
					$("#logOut").show();
					$(".BookDetails").show();
					$("#tblBody tr").remove();
				}
				else {
					alert("Invalid User name or Password");
					$("#checkEmail").val("");
					$("#checkPassword").val("");
				}
			}
		});
	});

	/////////////////////    User Profile    ////////////////////

	var getUser = function () {
		$.ajax({
			url: 'http://localhost:50477//api//User/' + id,
			method: 'GET',
			headers: {
				"Authorization": "Basic " + btoa(credentials)
			},
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var user = xmlhttp.responseJSON;
					// alert("Login Successful!!");
					$("#profileName").text(user.user_name);
					$("#profileEmail").text(user.email);
					$("#averageRatingUser").text((user.averageRating).toFixed(2));
					$("#totalRead").text(user.bookCount);
					$("#userImage").text(user.image_link);
				}
				else {
					alert("Login failed!!");
				}
			}
		});
	};

	// var getAverageRatingUser = function () {
	// 	$.ajax({
	// 		url: 'http://localhost:5507//api//UserBook/AverageRatingForUser/' + id,
	// 		method: 'GET',
	// 		headers: {
	// 			"Authorization": "Basic " + btoa(credentials)
	// 		},
	// 		complete: function (xmlhttp) {
	// 			if (xmlhttp.status == 200) {
	// 				var rating = xmlhttp.responseJSON;
	// 				// alert("Login Successful!!");
	// 				$("#averageRatingUser").text(rating);
	// 			}
	// 			else {
	// 				alert("Login failed!!");
	// 			}
	// 		}
	// 	});
	// };

	// var getTotalRead = function () {
	// 	$.ajax({
	// 		url: 'http://localhost:5507//api//UserBook/BookCount/' + id,
	// 		method: 'GET',
	// 		headers: {
	// 			"Authorization": "Basic " + btoa(credentials)
	// 		},
	// 		complete: function (xmlhttp) {
	// 			if (xmlhttp.status == 200) {
	// 				var rating = xmlhttp.responseJSON;
	// 				// alert("Login Successful!!");
	// 				$("#totalRead").text(rating);
	// 			}
	// 			else {
	// 				alert("Login failed!!");
	// 			}
	// 		}
	// 	});
	// };

	$("#editUserProfile").click(function () {
		hideAndSeek();
		$(".editUserContainer").show();
		//var id = e.target.getAttribute('data-id');
		//var id = 1;
		$.ajax({
			url: 'http://localhost:50477//api//User/' + parseInt(id),
			method: 'GET',
			headers: {
				"Authorization": "Basic " + btoa(credentials)
			},
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					var user = xmlhttp.responseJSON;
					$("#editUserName").val(user.user_name);
					$("#editUserPassword").val(user.password);
					$("#editUserId").val(user.userId);
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	});

	$("#editUserProfileBtn").click(function () {
		var id = $("#editUserId").val();
		$.ajax({
			url: 'http://localhost:50477//api//User/' + parseInt(id),
			method: 'PUT',
			headers: {
				"Authorization": "Basic " + btoa(credentials)
			},
			data: {
				"user_name": $("#editUserName").val(),
				"password": $("#editUserPassword").val(),
			},
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					$("#editUserName").val("");
					$("#editUserPassword").val("");
					$("#editUserId").val("");
					alert("Successfully edited");
					hideAndSeek();
					$(".UserProfileContainer").show();
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	});

	var populateBookForUser = function (bookList) {
		var outputStr = '';

		for (var i = 0; i < bookList.length; i++) {
			outputStr += '<tr><td scope="row">' + bookList[i].title + '</td><td>' + bookList[i].author + '</td><td>' + bookList[i].category + '</td><td>' + (bookList[i].review).toFixed(2) + '</td><td colspan="3">' + bookList[i].description + '</td><td><button class="btn btn-primary rateTheBook" data-id="' + bookList[i].bookId + '">Rating</button></td></tr>';
		}

		$('#profileTable').html(outputStr);
		//$(".BookDetails").show();
	};

	var fetchBook = function(value){
		$.ajax({
			url: 'http://localhost:50477//api//UserBook/UserBookList/' + parseInt(id) + '/' + parseInt(value),
			method: 'GET',
			headers: {
				"Authorization": "Basic " + btoa(credentials)
			},
			complete: function (xmlhttp) {
				if (xmlhttp.status == 200) {
					populateBookForUser(xmlhttp.responseJSON);
				}
				else {
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	};

	//$("#userOptionSelect").onchange()
	$("#userOptionSelect").on('change', function() {
		var value = ( parseInt($("#userOptionSelect").find(":selected").val() ));
		fetchBook(value);
	});
	//$("#userOptionSelect").on('change', fetchBook);

	$("#profile").click(function () {
		if (id == 0) {
			alert("Log in first");
			hideAndSeek();
			$(".LogInContainer").show();
		}
		else {
			hideAndSeek();
			getUser();
			//getAverageRatingUser();
			//getTotalRead();
			fetchBook(0);
			$(".UserProfileContainer").show();
		}
	});

});