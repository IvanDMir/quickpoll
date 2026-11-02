const http = require("http");
const express = require("express");
const { Pool } = require("pg");
const { Server } = require("socket.io");

const pool = new Pool({ connectionString: "postgres://postgres:postgres@db/postgres" });
const app = express();
const server = http.createServer(app);
const io = new Server(server);

app.use(express.static("public"));

async function sendTotals() {
  try {
    const { rows } = await pool.query("SELECT choice, COUNT(*)::int AS total FROM ballots GROUP BY choice");
    const totals = { a: 0, b: 0 };
    rows.forEach((row) => (totals[row.choice] = row.total));
    io.emit("totals", totals);
  } catch (err) {
    console.log("waiting for database...");
  }
}

setInterval(sendTotals, 1000);

server.listen(80, () => console.log("tally running on port 80"));
