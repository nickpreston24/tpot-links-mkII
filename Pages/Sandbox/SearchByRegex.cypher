MATCH (p)
WHERE "Page" IN LABELS(p) 
AND p.Title =~ '(?i)^P.*'
AND p.Slug =~ '(?i)^P.*'
RETURN p
