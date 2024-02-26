MATCH (paper:Paper)
MATCH (a)-[r:LINKS_TO]-(x)
WHERE 
    paper.Title =~ '(?is)(<\w+>)?.*(&nbsp;).*(<\w+>)?'
 OR 
   paper.Excerpt =~ '(?is)(<\w+>)?.*(&nbsp;).*(<\w+>)?'
OR 
paper.Content =~ '(?is)(<\w+>)?.*(&nbsp;).*(<\w+>)?'
RETURN paper,r, x, a
LIMIT 10