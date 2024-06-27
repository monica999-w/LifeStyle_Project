import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { List, ListItem, ListItemIcon, ListItemText, Divider, Drawer, Avatar, Typography, Box, Collapse, IconButton } from '@mui/material';
import { FaUser, FaUtensils, FaDumbbell, FaEnvelope, FaList, FaPlus, FaCalendarAlt } from 'react-icons/fa';
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

    useEffect(() => {
        const handleResize = () => {
            if (window.innerWidth < 768) {
                setSidebarOpen(false);
            } else {
                setSidebarOpen(true);
            }
        };

        window.addEventListener('resize', handleResize);

        handleResize();

        return () => {
            window.removeEventListener('resize', handleResize);
        };
    }, []);

    return (
        <Box sx={{ position: 'relative' }}>
            <Drawer
                variant="permanent"
                sx={{
                    width: sidebarOpen ? 300 : 100,
                    flexShrink: 0,
                    [`& .MuiDrawer-paper`]: { 
                        width: sidebarOpen ? 300 : 100, 
                        boxSizing: 'border-box', 
                        overflowX: 'hidden',
                        transition: 'width 0.3s'
                    },
                }}
            >
                <Box display="flex" flexDirection="column" alignItems="center" mt={2}>
                    <Avatar src={logo} alt="Logo" sx={{ width: sidebarOpen ? 100 : 70, height: sidebarOpen ? 100 : 70 }} onClick={() => navigate('/')} />
                    {sidebarOpen && role === 'User' && <Typography variant="h6" noWrap>{email || 'user'}</Typography>}
                </Box>
                <Divider />
                <List>
                    <ListItem button onClick={handleProfileNavigation}>
                        <ListItemIcon sx={{ minWidth: sidebarOpen ? 'auto' : 'unset' }}>
                            <FaUser size={sidebarOpen ? 24 : 30} />
                        </ListItemIcon>
                        {sidebarOpen && <ListItemText primary="Profile" />}
                    </ListItem>
                    <ListItem button onClick={handleMealNavigation}>
                        <ListItemIcon sx={{ minWidth: sidebarOpen ? 'auto' : 'unset' }}>
                            <FaUtensils size={sidebarOpen ? 24 : 30} />
                        </ListItemIcon>
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
                        <ListItemIcon sx={{ minWidth: sidebarOpen ? 'auto' : 'unset' }}>
                            <FaDumbbell size={sidebarOpen ? 24 : 30} />
                        </ListItemIcon>
                        {sidebarOpen && <ListItemText primary="Exercise" />}
                        {sidebarOpen && (exerciseMenuOpen ? <ExpandLess /> : <ExpandMore />)}
                    </ListItem>
                    <Collapse in={exerciseMenuOpen} timeout="auto" unmountOnExit>
                        <List component="div" disablePadding>
                            {role === 'User' ? (
                                <>
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
                    {role === 'User' && (
                        <>
                            <ListItem button onClick={() => navigate(routes.planner)}>
                                <ListItemIcon sx={{ minWidth: sidebarOpen ? 'auto' : 'unset' }}>
                                    <FaCalendarAlt size={sidebarOpen ? 24 : 30} />
                                </ListItemIcon>
                                {sidebarOpen && <ListItemText primary="Planner" />}
                            </ListItem>
                            <ListItem button onClick={() => navigate(routes.contact)}>
                                <ListItemIcon sx={{ minWidth: sidebarOpen ? 'auto' : 'unset' }}>
                                    <FaEnvelope size={sidebarOpen ? 24 : 30} />
                                </ListItemIcon>
                                {sidebarOpen && <ListItemText primary="Contact" />}
                            </ListItem>
                        </>
                    )}
                </List>
                <Divider />
                <Box display="flex" justifyContent="center" mt={2}>
                    <Logout sidebarOpen={sidebarOpen} />
                </Box>
            </Drawer>
            <IconButton
                onClick={handleSidebarToggle}
                sx={{
                    position: 'absolute',
                    top: 16,
                    right: sidebarOpen ? -15 : -15,
                    zIndex: 1300,
                    transform: 'translateY(0%)',
                    transition: 'transform 0.3s',
                    backgroundColor: 'grey',
                    borderRadius: '50%',
                    color: 'white',
                    boxShadow: 3,
                    '&:hover': {
                        backgroundColor: '#555',
                    },
                    width: 30,
                    height: 30,
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center'
                }}
            >
                {sidebarOpen ? <ChevronLeft /> : <ChevronRight />}
            </IconButton>
        </Box>
    );
};

export default Sidebar;

