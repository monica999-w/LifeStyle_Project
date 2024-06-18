import AuthProvider from './components/provider/AuthProvider';
import AppRoute from './components/route/AppRoutes';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import { NotificationProvider } from './components/provider/NotificationContext';

const theme = createTheme();

const App: React.FC = () => {
  return (
    <ThemeProvider theme={theme}>
      <AuthProvider>
      <NotificationProvider>
        <AppRoute />
      </NotificationProvider>
      </AuthProvider>
    </ThemeProvider>
  );
};

export default App;