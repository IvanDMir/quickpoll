# QuickPoll

A simple real-time voting app made of five containers.

```
ballot (Python) ──► Redis ──► counter (.NET) ──► PostgreSQL ──► tally (Node)
```

| Service   | Stack            | Port | What it does                          |
|-----------|------------------|------|---------------------------------------|
| `ballot`  | Python, Flask    | 5000 | Voting page, sends votes to Redis     |
| `counter` | .NET 8           | –    | Moves votes from Redis to PostgreSQL  |
| `tally`   | Node, Socket.IO  | 5001 | Shows live results                    |
| `redis`   | Redis 7          | –    | Vote queue                            |
| `db`      | PostgreSQL 16    | –    | Stores the votes                      |

Each browser can vote once. Voting again changes the previous answer.

## Run with Docker Compose

```bash
docker compose up --build
```

- Vote: http://localhost:5000
- Results: http://localhost:5001
