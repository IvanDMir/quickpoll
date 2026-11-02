import json
import uuid

from flask import Flask, make_response, render_template, request
from redis import Redis

app = Flask(__name__)
redis = Redis(host="redis", port=6379)


@app.route("/", methods=["GET", "POST"])
def index():
    voter_id = request.cookies.get("voter_id") or uuid.uuid4().hex
    selected = None

    if request.method == "POST":
        selected = request.form["choice"]
        redis.rpush("ballots", json.dumps({"voter_id": voter_id, "choice": selected}))

    response = make_response(render_template("index.html", selected=selected))
    response.set_cookie("voter_id", voter_id)
    return response


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=80)
