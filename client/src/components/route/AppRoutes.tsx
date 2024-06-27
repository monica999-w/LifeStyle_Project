import React, { ReactNode, useState } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate, useLocation } from 'react-router-dom';
import Sidebar from '../sidebar/Sidebar';
import { useAuth } from '../provider/AuthProvider';
import UserProfile from '../../pages/profile/userProfile/UserProfile';
import AddExercise from '../../pages/exercise/add/AddExercise';
import ExerciseFilter from '../../pages/exercise/filter/ExerciseFilter';
import ExerciseList from '../../pages/exercise/list-sort/ExerciseList';  
import Login from '../../pages/auth/Login';
import Register from '../../pages/auth/Register';
import { Container, Box } from '@mui/material';
import Home from '../../pages/home/Home';
import routes  from '../../components/route/routes';
import UserList from '../../pages/profile/list/UserList';
import ExerciseListFull from '../../pages/exercise/list-full/ExerciseListFull';
import CreateMeal from '../../pages/meal/add/AddMeals';
import MealList from '../../pages/meal/list/MealList';
import AdminMealTable from '../../pages/meal/list/AdminMealTable';
import AdminExerciseTable from '../../pages/exercise/list-full/AdminExerciseTable';
import Planner from '../../pages/planner/Planner';
import MealDetail from '../../pages/meal/detail/MealDetail';
import ExerciseDetails from '../../pages/exercise/details/ExerciseDetails';
import ContactPage from '../../pages/contact/ContactPage';

interface Exercise {
    id: number;
    name: string;
    durationInMinutes: number;
    description: string;
    videoLink: string;
    type: number;
    equipment: number;
    majorMuscle: number;
}


const ProtectedRoute = ({ element }: { element: JSX.Element }) => {
  const { token } = useAuth();
  return token ? element : <Navigate to={routes.login} />;
};

interface LayoutProps {
  children: ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const location = useLocation();
  const hideSidebarRoutes = [routes.login, routes.register];
  const shouldHideSidebar = hideSidebarRoutes.includes(location.pathname);

  return (
    <Box display="flex">
      {!shouldHideSidebar && <Sidebar />}
      <Container component="main" sx={{ flexGrow: 1, p: 3 }}>
        {children}
      </Container>
    </Box>
  );
};

const AppRoute: React.FC = () => {
  const [exercises, setExercises] = useState<Exercise[]>([]);
 


  return (
    <Router>
      <Layout>
        <Routes>
          <Route path = {routes.home} element={<Home />} />
          <Route path={routes.login}  element={<Login />} />
          <Route path={routes.register}  element={<Register />} />
          <Route path={routes.profile} element={<ProtectedRoute element={<UserProfile />} />} />
          <Route path={routes.addExercise}  element={<ProtectedRoute element={<AddExercise />} />} />  
          <Route path={routes.exerciseFilter}  element={<ProtectedRoute element={<ExerciseFilter setExercises={setExercises} />} />} />
          <Route path={routes.exerciseList}  element={<ProtectedRoute element={<ExerciseList exercises={exercises}  />} />} /> 
          <Route path={routes.userList} element={<ProtectedRoute element={<UserList />} />} />
          <Route path={routes.exerciseListFull} element={<ProtectedRoute element={<ExerciseListFull />} />} />
          <Route path={routes.addMeals} element={<ProtectedRoute element={<CreateMeal />} />} />
          <Route path={routes.listMeal} element={<ProtectedRoute element={<MealList />} />} />
          <Route path={routes.mealTable} element={<ProtectedRoute element={<AdminMealTable />} />} />
          <Route path={routes.exerciseTable} element={<ProtectedRoute element={<AdminExerciseTable />} />} />
          <Route path={routes.planner} element={<ProtectedRoute element={<Planner />} />} />
          <Route path={routes.contact} element={<ProtectedRoute element={<ContactPage />} />} />
          <Route path="/meal/:mealId" element={<ProtectedRoute element={<MealDetail />} />} />
          <Route path="/exercises/:exerciseId" element={<ProtectedRoute element={<ExerciseDetails />} />} />
        
          <Route path="*" element={<Navigate to={routes.home} />} />
        </Routes>
      </Layout>
    </Router>
  );
};

export default AppRoute;

