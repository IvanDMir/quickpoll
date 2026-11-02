const socket = io();

socket.on("totals", ({ a, b }) => {
  const total = a + b;
  const pctA = total ? Math.round((a / total) * 100) : 50;
  const pctB = 100 - pctA;

  document.getElementById("fill-a").style.width = pctA + "%";
  document.getElementById("fill-b").style.width = pctB + "%";
  document.getElementById("pct-a").textContent = pctA + "%";
  document.getElementById("pct-b").textContent = pctB + "%";
  document.getElementById("count-a").textContent = a + " votes";
  document.getElementById("count-b").textContent = b + " votes";
  document.getElementById("total").textContent = total + " votes in total";
});
