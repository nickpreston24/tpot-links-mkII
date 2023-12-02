MATCH (p:Page)
  WHERE "Page" IN LABELS(p)
  AND p.Content =~ '(?is)(<\w+>)?.*($term).*(<\w+>)?'
RETURN p,r