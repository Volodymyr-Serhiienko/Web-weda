// вращение картинки вселенной
var square = document.getElementById("picrot");
var angle = 0;
function rotate() {
	angle = (angle + 0.25) % 360;
	square.style.transform = "rotate(" + angle + "deg)";
	window.requestAnimationFrame(rotate);
}
var id = window.requestAnimationFrame(rotate);