MATCH (paper:Paper)
  where paper.Categories contains $category
return paper
  limit 10

//MATCH (paper:Paper)
//  WHERE paper.Categories CONTAINS // e.g., '491' for Chinese
//RETURN paper.Title, paper.Categories
//  LIMIT $limit