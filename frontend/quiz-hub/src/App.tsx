import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import ProtectedRoute from './guards/ProtectedRoute';
import GuestRoute from './guards/GuestRoute';
import AdminRoute from './guards/AdminRouteGuard';
import Navbar from './components/layout/Navbar';
import Header from './components/layout/Header';
import Footer from './components/layout/Footer';
import Home from './pages/Home/Home';
import Login from './pages/User/Login';
import Register from './pages/User/Register';
import Quiz from './pages/Quiz/Quiz';
import Results from './pages/Results/Results';
import MyResults from './pages/MyResults/MyResults';
import Leaderboard from './pages/Leaderboard/Leaderboard';
import AdminDashboard from './pages/AdminDashboard/AdminDashboard';
import AdminCategories from './pages/AdminCategories/AdminCategories';
import AdminQuizzes from './pages/AdminQuizzes/AdminQuizzes';
import AdminQuizForm from './pages/AdminQuizForm/AdminQuizForm';
import AdminQuestions from './pages/AdminQuestions/AdminQuestions';
import AdminResults from './pages/AdminResults/AdminResults';
import AdminQuestionForm from 'pages/AdminQuestionForm.tsx/AdminQuestionForm';

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <div className="flex flex-col min-h-screen">
          <Header />
          <Navbar />
          <main className="container mx-auto px-4 py-8 flex-grow">
            <Routes>
              <Route
                path="/"
                element={
                  <ProtectedRoute>
                    <Home />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/quiz/:quizId"
                element={
                  <ProtectedRoute>
                    <Quiz />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/results/:attemptId"
                element={
                  <ProtectedRoute>
                    <Results />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/my-results"
                element={
                  <ProtectedRoute>
                    <MyResults />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/leaderboard"
                element={
                  <ProtectedRoute>
                    <Leaderboard />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/admin"
                element={
                  <AdminRoute>
                    <AdminDashboard />
                  </AdminRoute>
                }
              />
              <Route
                path="/admin/categories"
                element={
                  <AdminRoute>
                    <AdminCategories />
                  </AdminRoute>
                }
              />
              <Route
                path="/admin/quizzes"
                element={
                  <AdminRoute>
                    <AdminQuizzes />
                  </AdminRoute>
                }
              />
              <Route
                path="/admin/quizzes/create"
                element={
                  <AdminRoute>
                    <AdminQuizForm />
                  </AdminRoute>
                }
              />
              <Route
                path="/admin/quizzes/edit/:quizId"
                element={
                  <AdminRoute>
                    <AdminQuizForm />
                  </AdminRoute>
                }
              />
              <Route
                path="/admin/quizzes/:quizId/questions"
                element={
                  <AdminRoute>
                    <AdminQuestions />
                  </AdminRoute>
                }
              />
              <Route
                path="/admin/quizzes/:quizId/questions/create"
                element={
                  <AdminRoute>
                    <AdminQuestionForm />
                  </AdminRoute>
                }
              />
              <Route
                path="/admin/quizzes/:quizId/questions/edit/:questionId"
                element={
                  <AdminRoute>
                    <AdminQuestionForm />
                  </AdminRoute>
                }
              />
              <Route
                path="/admin/results"
                element={
                  <AdminRoute>
                    <AdminResults />
                  </AdminRoute>
                }
              />
              <Route
                path="/login"
                element={
                  <GuestRoute>
                    <Login />
                  </GuestRoute>
                }
              />
              <Route
                path="/register"
                element={
                  <GuestRoute>
                    <Register />
                  </GuestRoute>
                }
              />
            </Routes>
          </main>
          <Footer />
        </div>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;