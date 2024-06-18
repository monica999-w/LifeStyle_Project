import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { List, ListItem, ListItemIcon, ListItemText, Divider, Drawer, Avatar, Typography, Box, Collapse, IconButton } from '@mui/material';
import { FaUser, FaUtensils, FaDumbbell, FaEnvelope, FaFilter, FaList, FaPlus, FaCalendarAlt } from 'react-icons/fa';
import { ExpandLess, ExpandMore, ChevronLeft, ChevronRight } from '@mui/icons-material';
import { useAuth } from '../provider/AuthProvider';
import logo from '../../assets/logo.png';
import Logout from '../../pages/auth/Logout';
import routes from '../../components/route/routes';

const Sidebar: React.FC = () => {
    const navigate = useNavigate();
    const { email, role } = useAuth();
    const [exerciseMenuOpen, setExerciseMenuOpen] = useState(false);
    const [mealMenuOpen, setMealMenuOpen] = useState(false);
    const [sidebarOpen, setSidebarOpen] = useState(true);

    const handleExerciseNavigation = () => {
        setExerciseMenuOpen(!exerciseMenuOpen);
    };

    const handleMealNavigation = () => {
        setMealMenuOpen(!mealMenuOpen);
    };

    const handleProfileNavigation = () => {
        if (role === 'User') {
            navigate(routes.profile);
        } else {
            navigate(routes.userList);
        }
    };

    const handleSidebarToggle = () => {
        setSidebarOpen(!sidebarOpen);
    };

    return (
        <Drawer
            variant="permanent"
            sx={{
                width: sidebarOpen ? 300 : 70,
                flexShrink: 0,
                [`& .MuiDrawer-paper`]: { width: sidebarOpen ? 300 : 70, boxSizing: 'border-box' },
            }}
        >
            <Box display="flex" flexDirection="column" alignItems="center" mt={2}>
                <Avatar src={logo} alt="Logo" sx={{ width: sidebarOpen ? 100 : 40, height: sidebarOpen ? 100 : 40 }} onClick={() => navigate('/')} />
                {sidebarOpen && <Typography variant="h6" noWrap>{email || 'user'}</Typography>}
                <IconButton onClick={handleSidebarToggle} sx={{ mt: 2 }}>
                    {sidebarOpen ? <ChevronLeft /> : <ChevronRight />}
                </IconButton>
            </Box>
            <Divider />
            <List>
                <ListItem button onClick={handleProfileNavigation}>
                    <ListItemIcon><FaUser /></ListItemIcon>
                    {sidebarOpen && <ListItemText primary="Profile" />}
                </ListItem>
                <ListItem button onClick={handleMealNavigation}>
                    <ListItemIcon><FaUtensils /></ListItemIcon>
                    {sidebarOpen && <ListItemText primary="Meals" />}
                    {sidebarOpen && (mealMenuOpen ? <ExpandLess /> : <ExpandMore />)}
                </ListItem>
                <Collapse in={mealMenuOpen} timeout="auto" unmountOnExit>
                    <List component="div" disablePadding>
                        {role === 'User' ? (
                            <ListItem button sx={{ pl: 4 }} onClick={() => navigate(routes.listMeal)}>
                                <ListItemIcon><FaList /></ListItemIcon>
                                {sidebarOpen && <ListItemText primary="List Meal" />}
                            </ListItem>
                        ) : (
                            <>
                                <ListItem button sx={{ pl: 4 }} onClick={() => navigate(routes.addMeals)}>
                                    <ListItemIcon><FaPlus /></ListItemIcon>
                                    {sidebarOpen && <ListItemText primary="Add Meal" />}
                                </ListItem>
                                <ListItem button sx={{ pl: 4 }} onClick={() => navigate(routes.mealTable)}>
                                    <ListItemIcon><FaList /></ListItemIcon>
                                    {sidebarOpen && <ListItemText primary="List Meal" />}
                                </ListItem>
                            </>
                        )}
                    </List>
                </Collapse>
                <ListItem button onClick={handleExerciseNavigation}>
                    <ListItemIcon><FaDumbbell /></ListItemIcon>
                    {sidebarOpen && <ListItemText primary="Exercise" />}
                    {sidebarOpen && (exerciseMenuOpen ? <ExpandLess /> : <ExpandMore />)}
                </ListItem>
                <Collapse in={exerciseMenuOpen} timeout="auto" unmountOnExit>
                    <List component="div" disablePadding>
                        {role === 'User' ? (
                            <>
                                <ListItem button sx={{ pl: 4 }} onClick={() => navigate(routes.exerciseFilter)}>
                                    <ListItemIcon><FaFilter /></ListItemIcon>
                                    {sidebarOpen && <ListItemText primary="Filter Workout" />}
                                </ListItem>
                                <ListItem button sx={{ pl: 4 }} onClick={() => navigate(routes.exerciseListFull)}>
                                    <ListItemIcon><FaList /></ListItemIcon>
                                    {sidebarOpen && <ListItemText primary="List Workout" />}
                                </ListItem>
                            </>
                        ) : (
                            <>
                                <ListItem button sx={{ pl: 4 }} onClick={() => navigate(routes.addExercise)}>
                                    <ListItemIcon><FaPlus /></ListItemIcon>
                                    {sidebarOpen && <ListItemText primary="Add Workout" />}
                                </ListItem>
                                <ListItem button sx={{ pl: 4 }} onClick={() => navigate(routes.exerciseTable)}>
                                    <ListItemIcon><FaList /></ListItemIcon>
                                    {sidebarOpen && <ListItemText primary="List Workout" />}
                                </ListItem>
                            </>
                        )}
                    </List>
                </Collapse>
                <ListItem button onClick={() => navigate(routes.planner)}>
                    <ListItemIcon><FaCalendarAlt /></ListItemIcon>
                    {sidebarOpen && <ListItemText primary="Planner" />}
                </ListItem>
                <ListItem button onClick={() => navigate('/contact')}>
                    <ListItemIcon><FaEnvelope /></ListItemIcon>
                    {sidebarOpen && <ListItemText primary="Contact" />}
                </ListItem>
            </List>
            <Divider />
            <Box display="flex" justifyContent="center" mt={2}>
                <Logout sidebarOpen={sidebarOpen} />
            </Box>
        </Drawer>
    );
};

export default Sidebar;

