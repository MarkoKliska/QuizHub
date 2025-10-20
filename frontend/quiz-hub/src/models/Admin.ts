export interface Category {
  id: string;
  name: string;
}

export interface CreateCategoryDto {
  name: string;
}

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
}

export interface CreateQuizDto {
  name: string;
  description: string;
  timeLimit: number;
  difficulty: string;
  categoryId: string;
}

export interface QuestionOption {
  id?: string;
  text: string;
  isCorrect: boolean;
}

export interface Question {
  id: string;
  quizId: string;
  text: string;
  type: string;
  points: number;
  correctAnswer?: string;
  options: QuestionOption[];
}

export interface CreateQuestionDto {
  quizId: string;
  text: string;
  type: string;
  points: number;
  correctAnswer?: string;
  options: QuestionOption[];
}