export interface Quiz {
  id: string;
  name: string;
  description: string;
  timeLimit: number;
  difficulty: string;
  categoryId: string;
  categoryName: string;
  createdBy: string;
  createdAt: string;
  numberOfQuestions: number; 
}