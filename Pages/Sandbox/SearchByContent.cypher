MATCH (p:Page)
  WHERE "Page" IN LABELS(p)
  AND p.Content =~ '(?is)(<\w+>)?.*(Proverb).*(<\w+>)?'
RETURN p,r