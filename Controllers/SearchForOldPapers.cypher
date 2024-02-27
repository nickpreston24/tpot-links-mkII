MATCH (n:Paper)
RETURN n.Id, n.Title, n.created, n.created_at_wp
ORDER BY n.created desc