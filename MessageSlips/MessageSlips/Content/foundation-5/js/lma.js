$(document).ready(function(){
	function removeActiveClass() {
		$("#home").removeClass("active");
		$("#courses").removeClass("active");
		$("#info").removeClass("active");
		$("#attendance").removeClass("active");
		$("#search").removeClass("active");
	}
	
	$("#submit").click(function submitForm() {
		var clientname = $('#input_client_name').val();
		var address = $('#input_address').val();
		var cityStateZip =  $('#input_cityStateZip').val();
		var email = $('#input_email').val();
		var phone = $('#input_phone').val();
		var tech = $('#input_tech').val();
		
		var d = new Date();
		var checkDay = d.getDay();
		// checks if it is Saturday or Sunday
		if (checkDay == 6 ) {d.setDate(d.getDate() + 2);}
		if (checkDay == 0 ) {d.setDate(d.getDate() + 1);}
		
		var day = d.getDate();
		var month = d.getMonth();
		var year = d.getFullYear();
		
		if (month > 10 || month < 3) {
			month = 3;
		}
		
		// adds leading 0's
		if (month < 10) {month = '0' + month;}
		if (day < 10) {day = '0' + day;}
		
		// Sets service date 1
		var serviceDate1 = year + '-' + month + '-' + day;
		
		// Checks for the next service date calculation using [checkDay] + 3 % 7
		d.setDate(d.getDate() + 3);
		checkDay = (checkDay + 3) % 7;
		alert(checkDay);
		// if checkDay is 0 then change it from Sunday to Monday
		if (checkDay == 0) {d.setDate(d.getDate() + 1);}
		
		day = d.getDate();
		month = d.getMonth();
		year = d.getFullYear();
		
		if (month > 10 || month < 3) {
			month = 3;
		}
		
		// adds leading 0's
		if (month < 10) {month = '0' + month;}
		if (day < 10) {day = '0' + day;}
		
		var serviceDate2 = year + '-' + month + '-' + day;
		
		if (clientname != "" && address != "" && cityStateZip != "") {
			$("#get_query_2").hide();
			$("#get_query_2").load("submit_create_client.php", {'clientname':clientname, 'address':address, 'cityStateZip':cityStateZip, 'email':email, 'phone':phone, 'tech':tech, 'serviceDate1':serviceDate1, 'serviceDate2':serviceDate2}, function(){
				$(this).fadeIn(500);
			});
			$("#get_query_2").parent().parent().parent().removeClass("hide");	
		}
		else {
			alert('Missing required entries' + serviceDate1 + serviceDate2);
		}
	});
	
	$("#login").click(function submitForm() {
		var userName = $('#user_name').val();
		var password = $('#password').val();
		
		if (userName == "mobile491" && password == "shamwow") {
			$("#login_form").fadeOut(800);
			$("#navbar").fadeIn(800);	
			$("#sign_in_success").fadeIn(1600);
		}
		else {
			alert('Missing required entries');
		}
	});
	
	$("#submit_course").click(function submitForm() {
		var courseNumber = $('#course_number').val();
		var courseName = $('#course_name').val();
		var instructorName =  $('#instructor_name').val();
		var courseDays = $('#course_days').val();
		var courseTime = $('#course_time').val();
		
		if (courseNumber != "" && courseName != "" && instructorName != "" && courseDays != "" && courseTime != "") {
			$("#get_query_2").hide();
			$("#get_query_2").load("submit_create_course.php", {'courseNumber':courseNumber, 'courseName':courseName, 'instructorName':instructorName, 'courseDays':courseDays, 'courseTime':courseTime}, function(){
				$(this).fadeIn(500);
			});
			$("#get_query_2").parent().parent().parent().removeClass("hide");	
		}
		else {
			alert('Missing required entries');
		}
	});
	
	$("#submit_announcement").click(function submitForm() {
		var courseNumber = $('#course_number').val();
		var courseName = $('#course_name').val();
		var instructorName =  $('#instructor_name').val();
		var message = $('#message').val();
		
		if (courseNumber != "" && courseName != "" && instructorName != "" && message != "") {
			$("#get_query_2").hide();
			$("#get_query_2").load("submit_create_announcement.php", {'courseNumber':courseNumber, 'courseName':courseName, 'instructorName':instructorName, 'message':message}, function(){
				$(this).fadeIn(500);
			});
			$("#get_query_2").parent().parent().parent().removeClass("hide");	
		}
		else {
			alert('Missing required entries');
		}
	});
	
	$("#home").click(function(){
		$("#main").hide();
		$("#main").load("home.txt", function(){
			$(this).fadeIn(500);
		});
		
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		removeActiveClass();
		$(this).addClass("active");
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});	
	
	$("#courses").click(function(){
		$("#main").hide();
		$("#main").load("courses.php", function(){
			$(this).fadeIn(500);
		});
		removeActiveClass();
		$(this).addClass("active");
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});	
	
	$("#info").click(function(){
		$("#main").hide();
		$("#main").load("info.php", function(){
			$(this).fadeIn(500);
		});
		removeActiveClass();
		$(this).addClass("active");
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
	
	$("#attendance").click(function(){
		$("#main").hide();
		$("#main").load("attendance.php", function(){
			$(this).fadeIn(500);
		});
		removeActiveClass();
		$(this).addClass("active");
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
	
	$("#search").click(function(){
		$("#main").hide();
		$("#main").load("search.php", function(){
			$(this).fadeIn(500);
		});
		removeActiveClass();
		$(this).addClass("active");
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
	
	$("#get_techs").click(function(){
		$("#get_query").hide();
		$("#get_query").load("get_techs.php", function(){
			$(this).fadeIn(500);
		});
		
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
	$("#get_clients").click(function(){
		$("#get_query").hide();
		$("#get_query").load("get_clients.php", function(){
			$(this).fadeIn(500);
		});
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});		
	$("#get_both").click(function(){
		$("#get_query").load("generate_json.php");
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});	
	$("#get_courses").click(function(){
		$("#get_query").hide();
		$("#get_query").load("get_courses.php", function(){
			$(this).fadeIn(500);
		});
		
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
	$("#get_announcements").click(function(){
		$("#get_query").hide();
		$("#get_query").load("get_announcements.php", function(){
			$(this).fadeIn(500);
		});
		
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
	$("#get_qr").click(function(){
		$("#get_query").hide();
		$("#get_query").load("get_qr.php", function(){
			$(this).fadeIn(500);
		});
		
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
	$("#create_course").click(function(){
		$("#get_query").hide();
		$("#get_query").load("create_course.php", function(){
			$(this).fadeIn(500);
		});
		
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
	$("#create_announcement").click(function(){
		$("#get_query").hide();
		$("#get_query").load("create_announcement.php", function(){
			$(this).fadeIn(500);
		});
		
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
	$("#search_invoices").click(function(){
		$("#get_query").hide();
		$("#get_query").load("search_invoices.php", function(){
			$(this).fadeIn(500);
		});
		
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
	$("#show_attendance").click(function(){
		$("#get_query").hide();
		$("#get_query").load("show_attendance.php", function(){
			$(this).fadeIn(500);
		});
		
		//$("#get_query").load("get_techs.php").hide().fadeIn('slow');
		$("#get_query_2").parent().parent().parent().addClass("hide");
	});
});