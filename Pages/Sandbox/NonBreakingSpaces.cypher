MATCH (paper:Paper)
MATCH (a)-[r:LINKS_TO]-(x)
  WHERE
  paper.Title =~ '(&nbsp;)'
  OR
  paper.Excerpt =~ '(&nbsp;)'
  OR
  paper.Content =~ '(&nbsp;)'
RETURN paper, r, x, a
  LIMIT 10