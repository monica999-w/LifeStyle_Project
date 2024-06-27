import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, IconButton, Tooltip } from '@mui/material';
import { FaSignOutAlt } from 'react-icons/fa';
import { useAuth } from '../../components/provider/AuthProvider';
import routes from '../../components/route/routes';

interface LogoutProps {
    sidebarOpen: boolean;
}

const Logout: React.FC<LogoutProps> = ({ sidebarOpen }) => {
    const { setToken } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        setToken(null);
        navigate(routes.login);
    };

    return (
        <Tooltip title="Logout" placement="right">
            <div>
                {sidebarOpen ? (
                    <Button
                        variant="contained"
                        color="secondary"
                        startIcon={<FaSignOutAlt />}
                        onClick={handleLogout}
                        sx={{
                            borderRadius: 8,
                            backgroundColor: 'red',
                            '&:hover': {
                                backgroundColor: 'darkred', // schimbarea culorii la hover
                            },
                        }}
                    >
                        Logout
                    </Button>
                ) : (
                    <IconButton
                        color="secondary"
                        onClick={handleLogout}
                        sx={{
                            borderRadius: 8,
                            backgroundColor: 'red',
                            '&:hover': {
                                backgroundColor: 'darkred', 
                            },
                        }}
                    >
                        <FaSignOutAlt style={{ color: 'white' }} />
                    </IconButton>
                )}
            </div>
        </Tooltip>
    );
};

export default Logout;
