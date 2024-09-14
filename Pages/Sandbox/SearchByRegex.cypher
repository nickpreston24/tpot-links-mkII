MATCH (paper:Paper)
// OPTIONAL MATCH (a)-[r:LINKS_TO]-(x)
  WHERE
  paper.title =~ $regex OR paper.Title =~ $regex
//  OR 
//    paper.Excerpt =~ $regex
//  OR 
//    paper.Slug =~ $regex
//OR 
// paper.Content =~ $regex
//  OR paper.Id = $id
//   OR paper.Categories =~ $regex
RETURN paper
  ORDER BY paper.created DESC, paper.Title

  LIMIT $limit


// MATCH (paper:Paper)-[r]->(m)
// WHERE 'Paper' IN labels(paper) 
//  AND
//   (paper.Title =~ $regex OR toLower(paper.Title) CONTAINS toLower($term)
//     OR paper.Slug =~ $regex OR toLower(paper.Slug) CONTAINS toLower($term)
//     OR paper.Excerpt =~ $regex OR toLower(paper.Excerpt) CONTAINS toLower($term)
//   )
//   AND ($category = '' OR toLower(paper.Categories) contains toLower($category))
// RETURN paper, r, m


// //        AND (:EMP_ID IS NULL OR "EMPLOYEE_ID"=:EMP_ID) 
