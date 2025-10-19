export interface Question {
  id: string;
  quizId: string;
  text: string;
  type: 'SingleChoice' | 'MultipleChoice' | 'TrueFalse' | 'FillInBlank';
  points: number;
  correctAnswer: string | null;
  options: Option[];
}

export interface Option {
  id: string;
  text: string;
  isCorrect: boolean;
}