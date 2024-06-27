import React, { useEffect, useState } from 'react';
import { Card, CardContent, CardMedia, Typography, Grid, Box, Button, IconButton, Dialog, DialogTitle, DialogContent, DialogActions } from '@mui/material';
import axios from 'axios';
import { useAuth } from '../../components/provider/AuthProvider';
import { environment } from '../../environments/environment';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import { useNavigate } from 'react-router-dom';
import DeleteIcon from '@mui/icons-material/Delete';
import { styled } from '@mui/system';
import { useNotification } from '../../components/provider/NotificationContext';

interface Nutrients {
    calories: number;
    protein: number;
    carbohydrates: number;
    fat: number;
}

interface Meal {
    mealId: number;
    mealName: string;
    image: string;
    description: string;
    nutrients: Nutrients; // Adding nutrients property
}

interface Exercise {
    exerciseId: number;
    exerciseName: string;
    durationInMinutes: number;
    videoLink: string;
    description: string;
}

interface Planner {
    plannerId: number;
    meals: Meal[];
    exercises: Exercise[];
    date: string;
}

const StyledCalendar = styled(Calendar)({
    width: '100%',
    borderRadius: '10px',
    border: '1px solid #ddd',
    padding: '10px',
    boxShadow: '0 2px 5px rgba(0, 0, 0, 0.1)',
    marginTop: '20px'
});

const Planner: React.FC = () => {
    const { token, email } = useAuth();
    const navigate = useNavigate();
    const { notify } = useNotification();
    const [planner, setPlanner] = useState<Planner | null>(null);
    const [date, setDate] = useState<Date>(new Date());
    const [availableDates, setAvailableDates] = useState<Date[]>([]);
    const [showCalendar, setShowCalendar] = useState(true);
    const [confirmOpen, setConfirmOpen] = useState(false);
    const [confirmType, setConfirmType] = useState<'meal' | 'exercise' | null>(null);
    const [confirmId, setConfirmId] = useState<number | null>(null);

    const fetchPlanner = async (selectedDate: Date) => {
        try {
            const formattedDate = new Date(Date.UTC(
                selectedDate.getFullYear(),
                selectedDate.getMonth(),
                selectedDate.getDate()
            )).toISOString().split('T')[0]; 
            console.log('Fetching planner for date:', formattedDate);
            const response = await axios.get<Planner>(`${environment.apiUrl}Planner/user/${email}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                },
                params: { date: formattedDate } 
            });
            console.log('Planner response:', response.data);
            setPlanner(response.data);
        } catch (error) {
            console.error('Failed to fetch planner:', error);
            notify('Failed to fetch planner', 'error');
            setPlanner(null);
        }
    };

    const deleteMealFromPlanner = async (mealId: number) => {
        if (!planner) return;

        try {
            const response = await axios.delete(`${environment.apiUrl}Planner/${planner.plannerId}/meal/${mealId}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            console.log('Deleted meal from planner:', response.data);
            setPlanner({
                ...planner,
                meals: planner.meals.filter(meal => meal.mealId !== mealId)
            });
            notify('Meal deleted successfully', 'success');
        } catch (error) {
            console.error('Failed to delete meal from planner:', error);
            notify('Failed to delete meal from planner', 'error');
        }
    };

    const deleteExerciseFromPlanner = async (exerciseId: number) => {
        if (!planner) return;

        try {
            const response = await axios.delete(`${environment.apiUrl}Planner/${planner.plannerId}/exercise/${exerciseId}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            console.log('Deleted exercise from planner:', response.data);
            setPlanner({
                ...planner,
                exercises: planner.exercises.filter(exercise => exercise.exerciseId !== exerciseId)
            });
            notify('Exercise deleted successfully', 'success');
        } catch (error) {
            console.error('Failed to delete exercise from planner:', error);
            notify('Failed to delete exercise from planner', 'error');
        }
    };

    useEffect(() => {
        fetchPlanner(date);
    }, [date, token, email]);

    useEffect(() => {
        const fetchAvailableDates = async () => {
            try {
                const response = await axios.get<string[]>(`${environment.apiUrl}Planner/user/${email}/dates`, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                const dates = response.data.map(dateString => new Date(dateString));
                console.log('Available dates:', dates);
                setAvailableDates(dates);
            } catch (error) {
                console.error('Failed to fetch available dates:', error);
                notify('Failed to fetch available dates', 'error');
            }
        };

        fetchAvailableDates();
    }, [token, email]);

    const handleDateChange = (newDate: Date) => {
        console.log('Date selected:', newDate);
        setDate(newDate);
        fetchPlanner(newDate);
    };

    const handleGoToMeal = (mealId: number) => {
        navigate(`/meal/${mealId}`);
    };

    const handleGoToExercise = (exerciseId: number) => {
        navigate(`/exercises/${exerciseId}`);
    };

    const isToday = (dateToCheck: Date) => {
        const today = new Date();
        return dateToCheck.toDateString() === today.toDateString();
    };

    const handleOpenConfirm = (type: 'meal' | 'exercise', id: number) => {
        setConfirmType(type);
        setConfirmId(id);
        setConfirmOpen(true);
    };

    const handleCloseConfirm = () => {
        setConfirmOpen(false);
        setConfirmType(null);
        setConfirmId(null);
    };

    const handleConfirmDelete = () => {
        if (confirmType === 'meal' && confirmId !== null) {
            deleteMealFromPlanner(confirmId);
        } else if (confirmType === 'exercise' && confirmId !== null) {
            deleteExerciseFromPlanner(confirmId);
        }
        handleCloseConfirm();
    };

    // Log planner meals to debug
    console.log('Planner meals:', planner?.meals);

    const totalCalories = planner?.meals.reduce((acc, meal) => {
        console.log('Meal nutrients:', meal.nutrients); // Log nutrients of each meal
        return acc + (meal.nutrients?.calories || 0);
    }, 0) || 0;

    return (
        <Box sx={{ padding: 2 }}>
           
            {planner ? (
                <Box>
                    <Typography variant="h4" gutterBottom>Planner for {new Date(planner.date).toDateString()}</Typography>
                    <Box sx={{ textAlign: 'center' }}>
                     <Button variant="contained" onClick={() => setShowCalendar(!showCalendar)} sx={{ marginTop: 2 }}>
                     {showCalendar ? 'Hide Calendar' : 'Show Calendar'}
                     </Button>
                     {showCalendar && (
                      <StyledCalendar
                        onChange={(newDate) => handleDateChange(newDate as Date)}
                        value={date}
                        tileDisabled={({ date }) => !availableDates.some(d => d.toDateString() === date.toDateString())}
                      />
                )}
            </Box>
                    <Box sx={{ marginTop: 2, padding: 2, border: '1px solid #ddd', borderRadius: '10px', boxShadow: '0 2px 5px rgba(0,0,0,0.1)' }}>
                        <Typography variant="h6">Total Calories for Today: {totalCalories}</Typography>
                    </Box>
                    <Typography variant="h5" gutterBottom>Meals</Typography>
                    <Grid container spacing={2}>
                        {planner.meals.map((meal) => (
                            <Grid item key={meal.mealId} xs={12} sm={6} md={4}>
                                <Card sx={{ border: '1px solid #ddd', boxShadow: '0 2px 5px rgba(0,0,0,0.1)' }}>
                                    <CardContent sx={{ textAlign: 'center' }}>
                                        <Typography variant="h6">{meal.mealName}</Typography>
                                        <CardMedia
                                            component="img"
                                            height="140"
                                            image={`${'https://localhost:44353/'}/${meal.image}`}
                                            alt={meal.mealName}
                                            sx={{ margin: '10px auto' }}
                                        />
                                        <Button
                                            variant="contained"
                                            color="primary"
                                            onClick={() => handleGoToMeal(meal.mealId)}
                                            sx={{
                                                marginTop: '10px',
                                                backgroundColor: '#00695c',
                                                '&:hover': {
                                                    backgroundColor: '#004d40',
                                                },
                                                color: '#fff',
                                                padding: '10px 20px',
                                                fontSize: '16px',
                                                borderRadius: '5px',
                                                boxShadow: '0 3px 5px rgba(0,0,0,0.2)',
                                            }}
                                        >
                                            Go Recipe
                                        </Button>
                                        {isToday(new Date(planner.date)) && (
                                            <IconButton
                                                onClick={() => handleOpenConfirm('meal', meal.mealId)}
                                                sx={{
                                                    marginTop: '10px',
                                                    color: '#ff4081',
                                                    '&:hover': {
                                                        color: '#c60055',
                                                    },
                                                }}
                                            >
                                                <DeleteIcon />
                                            </IconButton>
                                        )}
                                    </CardContent>
                                </Card>
                            </Grid>
                        ))}
                    </Grid>
                    <Typography variant="h5" gutterBottom>Exercises</Typography>
                    <Grid container spacing={2}>
                        {planner.exercises.map((exercise) => (
                            <Grid item key={exercise.exerciseId} xs={12} sm={6} md={4}>
                                <Card sx={{ border: '1px solid #ddd', boxShadow: '0 2px 5px rgba(0,0,0,0.1)' }}>
                                    <CardContent sx={{ textAlign: 'center' }}>
                                        <Typography variant="h6">{exercise.exerciseName}</Typography>
                                        <Typography color="text.secondary">
                                            {`Duration: ${exercise.durationInMinutes} mins`}
                                        </Typography>
                                        {exercise.videoLink && (
                                            <iframe
                                                width="100%"
                                                height="200"
                                                src={exercise.videoLink.replace("watch?v=", "embed/")}
                                                title={exercise.exerciseName}
                                                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                                allowFullScreen
                                            ></iframe>
                                        )}
                                        <Button
                                            variant="contained"
                                            color="primary"
                                            onClick={() => handleGoToExercise(exercise.exerciseId)}
                                            sx={{
                                                marginTop: '10px',
                                                backgroundColor: '#00695c',
                                                '&:hover': {
                                                    backgroundColor: '#004d40',
                                                },
                                                color: '#fff',
                                                padding: '10px 20px',
                                                fontSize: '16px',
                                                borderRadius: '5px',
                                                boxShadow: '0 3px 5px rgba(0,0,0,0.2)',
                                            }}
                                        >
                                            Read More
                                        </Button>
                                        {isToday(new Date(planner.date)) && (
                                            <IconButton
                                                onClick={() => handleOpenConfirm('exercise', exercise.exerciseId)}
                                                sx={{
                                                    marginTop: '10px',
                                                    color: '#ff4081',
                                                    '&:hover': {
                                                        color: '#c60055',
                                                    },
                                                }}
                                            >
                                                <DeleteIcon />
                                            </IconButton>
                                        )}
                                    </CardContent>
                                </Card>
                            </Grid>
                        ))}
                    </Grid>
                </Box>
            ) : (
                <Box sx={{ textAlign: 'center' }}>
                    <Typography variant="h6" align="center" sx={{ marginTop: 4, color: 'red' }}>
                        No planner found for {date.toDateString()}.
                    </Typography>
                </Box>
            )}

            <Dialog
                open={confirmOpen}
                onClose={handleCloseConfirm}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title" sx={{ backgroundColor: '#f44336', color: '#fff' }}>
                    Are you sure you want to delete this?
                </DialogTitle>
                <DialogContent sx={{ padding: '16px 24px' }}>
                    <Typography variant="body1">Do you really want to delete this {confirmType} from the planner?</Typography>
                </DialogContent>
                <DialogActions sx={{ padding: '8px 24px' }}>
                    <Button onClick={handleCloseConfirm} variant="outlined" color="primary">
                        Cancel
                    </Button>
                    <Button onClick={handleConfirmDelete} variant="contained" color="secondary" sx={{ backgroundColor: '#d32f2f' }}>
                        Confirm
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
};

export default Planner;
