import React, { useEffect, useState } from 'react';
import { IconButton, CardMedia, Typography, Box, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { environment } from '../../../environments/environment';
import { useAuth } from '../../../components/provider/AuthProvider';
import { useNotification } from '../../../components/provider/NotificationContext';
import LocalDiningIcon from '@mui/icons-material/LocalDining';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import RestaurantIcon from '@mui/icons-material/Restaurant';
import AllergyIcon from '@mui/icons-material/Coronavirus';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import FavoriteIcon from '@mui/icons-material/Favorite';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import routes from '../../../components/route/routes';

const mealTypes = [
    { value: 0, label: 'Morning' },
    { value: 1, label: 'Noon' },
    { value: 2, label: 'Evening' },
];

const dietTypes = [
    { value: 0, label: 'NoDiet' },
    { value: 1, label: 'LactoVegetarian' },
    { value: 2, label: 'OvoVegetarian' },
    { value: 3, label: 'Paleo' },
    { value: 4, label: 'Primal' },
    { value: 5, label: 'Pescetarian' },
    { value: 6, label: 'Vegan' },
    { value: 7, label: 'Vegetarian' },
    { value: 8, label: 'Whole30' },
];

const allergyTypes = [
    { value: 0, label: 'Dairy' },
    { value: 1, label: 'Egg' },
    { value: 2, label: 'Gluten' },
    { value: 3, label: 'Peanut' },
    { value: 4, label: 'Seafood' },
    { value: 5, label: 'Sesame' },
    { value: 6, label: 'Shellfish' },
    { value: 7, label: 'Soy' },
    { value: 8, label: 'TreeNut' },
    { value: 9, label: 'Wheat' },
    { value: 10, label: 'Corn' },
];

interface Nutrients {
    calories: number;
    protein: number;
    carbohydrates: number;
    fat: number;
}

interface Meal {
    mealId: number;
    mealName: string;
    mealType: number;
    nutrients: Nutrients;
    preparationInstructions: string;
    estimatedPreparationTimeInMinutes: number;
    image: string;
    ingredients: string[];
    diets: number[];
    allergies: number[];
}

const MealDetails: React.FC = () => {
    const { token, email } = useAuth();
    const { notify } = useNotification();
    const { mealId } = useParams<{ mealId: string }>();
    const navigate = useNavigate();
    const [meal, setMeal] = useState<Meal | null>(null);
    const [plannerMeals, setPlannerMeals] = useState<number[]>([]);

    const fetchMeal = async () => {
        try {
            const response = await axios.get<Meal>(`${environment.apiUrl}Meal/${mealId}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setMeal(response.data);
        } catch (error) {
            console.error('Failed to fetch meal:', error);
        }
    };

    const fetchPlanner = async () => {
        try {
            const date = new Date().toISOString().split('T')[0];
            const response = await axios.get(`${environment.apiUrl}Planner/user/${email}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                },
                params: { date }
            });
            const mealIds = response.data.meals.map((meal: Meal) => meal.mealId);
            setPlannerMeals(mealIds);
        } catch (error) {
            console.error('Failed to fetch planner:', error);
        }
    };

    useEffect(() => {
        fetchMeal();
        fetchPlanner();
    }, [mealId, token]);

    const handleAddToPlanner = async () => {
        if (!meal) return;

        try {
            const date = new Date().toISOString();
            const response = await axios.post(`${environment.apiUrl}Planner`, {
                profile: email,
                date,
                mealIds: [meal.mealId],
                exerciseIds: []
            }, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                }
            });
            console.log('Added to planner:', response.data);
            setPlannerMeals([...plannerMeals, meal.mealId]);
            notify('Meal added to planner!', 'success');
        } catch (error) {
            console.error('Failed to add to planner:', error);
            notify('Failed to add meal to planner', 'error');
        }
    };

    const isMealInPlanner = (mealId: number) => plannerMeals.includes(mealId);

    return (
        <Box sx={{ padding: 2 }}>
            {meal ? (
                <Box>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
                        <IconButton onClick={() => navigate(routes.listMeal)} sx={{ color: '#00695c' }}>
                            <ArrowBackIcon />
                        </IconButton>
                        <IconButton
                            onClick={handleAddToPlanner}
                            sx={{
                                fontSize: 20,
                                color: isMealInPlanner(meal.mealId) ? '#ff4081' : '#ddd',
                                '&:hover': {
                                    color: isMealInPlanner(meal.mealId) ? '#c60055' : '#ff4081',
                                },
                            }}
                        >
                            {isMealInPlanner(meal.mealId) ? <FavoriteIcon /> : <FavoriteBorderIcon />}
                            Add Your Planner
                        </IconButton>
                    </Box>
                    <Typography id="meal-modal-title" variant="h4" component="h2" align="center" sx={{ backgroundColor: '#00695c', color: '#fff', padding: '8px 0', marginBottom: '20px' }}>
                        {meal.mealName}
                    </Typography>
                    <CardMedia
                        component="img"
                        sx={{ width: '400px', height: '300px', margin: '0 auto', borderRadius: '10px' }}
                        image={`https://localhost:44353/${meal.image}`}
                        alt={meal.mealName}
                    />
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '20px' }}>
                        <Typography variant="h6" align="center">
                            <LocalDiningIcon /> {mealTypes[meal.mealType].label}
                        </Typography>
                        <Typography variant="h6" align="center">
                            <AccessTimeIcon /> {meal.estimatedPreparationTimeInMinutes} mins
                        </Typography>
                    </Box>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '20px' }}>
                        <Typography variant="h6" align="center">
                            <RestaurantIcon /> {meal.diets.map(diet => dietTypes[diet].label).join(', ')}
                        </Typography>
                        <Typography variant="h6" align="center">
                            <AllergyIcon /> {meal.allergies.map(allergy => allergyTypes[allergy].label).join(', ')}
                        </Typography>
                    </Box>
                    <Typography variant="body1" paragraph sx={{ backgroundColor: '#e8f5e9', padding: '10px', borderRadius: '5px', marginTop: '20px' }}>
                        {meal.preparationInstructions}
                    </Typography>
                    <Typography variant="h6" gutterBottom>
                        Ingredient List:
                    </Typography>
                    <Box sx={{ marginLeft: '20px' }}>
                        {meal.ingredients.map((ingredient, index) => (
                            <Typography key={index} variant="body1" sx={{ marginBottom: '5px', display: 'flex', alignItems: 'center' }}>
                                <Box sx={{
                                    width: '24px',
                                    height: '24px',
                                    backgroundColor: '#00695c',
                                    color: '#fff',
                                    borderRadius: '50%',
                                    display: 'flex',
                                    alignItems: 'center',
                                    justifyContent: 'center',
                                    marginRight: '10px'
                                }}>
                                    {index + 1}
                                </Box>
                                {ingredient}
                            </Typography>
                        ))}
                    </Box>
                    <Typography variant="h6" gutterBottom sx={{ marginTop: '20px' }}>
                        Nutritional values:
                    </Typography>
                    <TableContainer component={Paper} sx={{
                        marginTop: '10px',
                        '&::-webkit-scrollbar': {
                            width: '0.4em'
                        },
                        '&::-webkit-scrollbar-track': {
                            boxShadow: 'inset 0 0 6px rgba(0,0,0,0.1)'
                        },
                        '&::-webkit-scrollbar-thumb': {
                            backgroundColor: '#00695c',
                            borderRadius: '10px',
                        }
                    }}>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell sx={{ backgroundColor: '#00695c', color: '#fff' }}>Nutrient</TableCell>
                                    <TableCell align="right" sx={{ backgroundColor: '#00695c', color: '#fff' }}>Amount</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                <TableRow>
                                    <TableCell sx={{ backgroundColor: '#e8f5e9' }}>Calories</TableCell>
                                    <TableCell align="right" sx={{ backgroundColor: '#e8f5e9' }}>{meal.nutrients.calories} kcal</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell sx={{ backgroundColor: '#e8f5e9' }}>Protein</TableCell>
                                    <TableCell align="right" sx={{ backgroundColor: '#e8f5e9' }}>{meal.nutrients.protein} g</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell sx={{ backgroundColor: '#e8f5e9' }}>Carbohydrates</TableCell>
                                    <TableCell align="right" sx={{ backgroundColor: '#e8f5e9' }}>{meal.nutrients.carbohydrates} g</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell sx={{ backgroundColor: '#e8f5e9' }}>Fat</TableCell>
                                    <TableCell align="right" sx={{ backgroundColor: '#e8f5e9' }}>{meal.nutrients.fat} g</TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </TableContainer>
                </Box>
            ) : (
                <Typography variant="h6" align="center" sx={{ marginTop: 4, color: 'red' }}>
                    No meal found.
                </Typography>
            )}
        </Box>
    );
};

export default MealDetails;
