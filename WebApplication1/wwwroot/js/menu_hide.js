// скрытие/появление главного меню
var prevScrollpos = window.pageYOffset;
window.onscroll = function () {
	var currentScrollPos = window.pageYOffset;
	if (prevScrollpos > currentScrollPos) {
		document.getElementById("menu").style.top = "0";
	} else {
		document.getElementById("menu").style.top = "-60px";
	}
	prevScrollpos = currentScrollPos;
}