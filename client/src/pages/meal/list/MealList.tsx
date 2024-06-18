import React, { useEffect, useState } from 'react';
import { Pagination,Card, CardContent, CardMedia, Typography, Grid, Box, Button, IconButton, FormControl, InputLabel, Select, MenuItem, SelectChangeEvent, TextField, Divider } from '@mui/material';
import axios from 'axios';
import { useAuth } from '../../../components/provider/AuthProvider';
import { environment } from '../../../environments/environment';
import { useNavigate } from 'react-router-dom';
import FilterListIcon from '@mui/icons-material/FilterList';
import SearchIcon from '@mui/icons-material/Search';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import FavoriteIcon from '@mui/icons-material/Favorite';


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

interface PagedResult<T> {
    items: T[];
    totalCount: number;
    pageSize: number;
    pageNumber: number;
}

const MealList: React.FC = () => {
    const { token, email } = useAuth();
    const [, setMeals] = useState<Meal[]>([]);
    const [filteredMeals, setFilteredMeals] = useState<Meal[]>([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize] = useState(10);
    const [totalPages, setTotalPages] = useState(1);
    const [filter, setFilter] = useState({ type: '', diet: '', allergy: '', maxCalories: '' });
    const [isFilterApplied, setIsFilterApplied] = useState(false);
    const [searchTerm, setSearchTerm] = useState('');
    const [plannerMeals, setPlannerMeals] = useState<number[]>([]); 
    const navigate = useNavigate();

    const fetchMeals = async () => {
        try {
            const response = await axios.get<PagedResult<Meal>>(`${environment.apiUrl}Meal`, {
                headers: {
                    Authorization: `Bearer ${token}`
                },
                params: {
                    pageNumber,
                    pageSize
                }
            });
            setMeals(response.data.items);
            setFilteredMeals(response.data.items);
            setTotalPages(Math.ceil(response.data.totalCount / pageSize));
        } catch (error) {
            console.error('Failed to fetch meals:', error);
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
        fetchMeals();
        fetchPlanner(); 
    }, [pageNumber, pageSize, token]);

    const handleFilterChange = async (e: SelectChangeEvent) => {
        const newFilter = { ...filter, [e.target.name]: e.target.value };
        setFilter(newFilter);
        setIsFilterApplied(true);

        try {
            const response = await axios.get(`${environment.apiUrl}Meal/filter`, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                },
                params: newFilter
            });
            setFilteredMeals(response.data);
            setPageNumber(1);
        } catch (error) {
            console.error('Failed to filter meals:', error);
        }
    };

    const handleClearFilter = async () => {
        setFilter({ type: '', diet: '', allergy: '', maxCalories: '' });
        setIsFilterApplied(false);
        await fetchMeals();
    };

    const handlePageChange = (_event: React.ChangeEvent<unknown>, value: number) => {
        setPageNumber(value);
    };

    const handleSearch = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        try {
            const response = await axios.get(`${environment.apiUrl}Meal/search`, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                },
                params: { searchTerm }
            });
            setFilteredMeals(response.data);
            setPageNumber(1);
        } catch (error) {
            console.error('Failed to search meals:', error);
        }
    };

    const handleAddToPlanner = async (mealId: number) => {
        try {
            const date = new Date().toISOString(); 
            const response = await axios.post(`${environment.apiUrl}Planner`, {
                profile: email,
                date,
                mealIds: [mealId],
                exerciseIds: [] 
            }, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                }
            });
            console.log('Added to planner:', response.data);
            setPlannerMeals([...plannerMeals, mealId]); 
        } catch (error) {
            console.error('Failed to add to planner:', error);
        }
    };

    const isMealInPlanner = (mealId: number) => plannerMeals.includes(mealId);

    const handleNavigateToDetails = (mealId: number) => {
        navigate(`/meal/${mealId}`);
    };

    const paginatedMeals = filteredMeals.slice((pageNumber - 1) * pageSize, pageNumber * pageSize);

    return (
        <Box sx={{ padding: 2 }}>
            <Box display="flex" justifyContent="center" alignItems="center" mt={2} mb={3}>
                <FilterListIcon sx={{ mr: 2, color: '#00695c' }} />
                <FormControl variant="outlined" sx={{ m: 1, minWidth: 120, borderRadius: '16px' }}>
                    <InputLabel>Type</InputLabel>
                    <Select name="type" value={filter.type} onChange={handleFilterChange} label="Type" sx={{ borderRadius: '16px' }}>
                        <MenuItem value=""><em>None</em></MenuItem>
                        {mealTypes.map((type) => (
                            <MenuItem key={type.value} value={type.value}>{type.label}</MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <FormControl variant="outlined" sx={{ m: 1, minWidth: 120, borderRadius: '16px' }}>
                    <InputLabel>Diet</InputLabel>
                    <Select name="diet" value={filter.diet} onChange={handleFilterChange} label="Diet" sx={{ borderRadius: '16px' }}>
                        <MenuItem value=""><em>None</em></MenuItem>
                        {dietTypes.map((diet) => (
                            <MenuItem key={diet.value} value={diet.value}>{diet.label}</MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <FormControl variant="outlined" sx={{ m: 1, minWidth: 120, borderRadius: '16px' }}>
                    <InputLabel>Allergy</InputLabel>
                    <Select name="allergy" value={filter.allergy} onChange={handleFilterChange} label="Allergy" sx={{ borderRadius: '16px' }}>
                        <MenuItem value=""><em>None</em></MenuItem>
                        {allergyTypes.map((allergy) => (
                            <MenuItem key={allergy.value} value={allergy.value}>{allergy.label}</MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <FormControl variant="outlined" sx={{ m: 1, minWidth: 150, borderRadius: '16px' }}>
                    <InputLabel>Max Calories</InputLabel>
                    <Select name="maxCalories" value={filter.maxCalories} onChange={handleFilterChange} label="Max Calories" sx={{ borderRadius: '16px' }}>
                        <MenuItem value=""><em>None</em></MenuItem>
                        <MenuItem value="200">200</MenuItem>
                        <MenuItem value="300">300</MenuItem>
                        <MenuItem value="400">400</MenuItem>
                        <MenuItem value="500">500</MenuItem>
                    </Select>
                </FormControl>
                {isFilterApplied && (
                    <Button variant="outlined" onClick={handleClearFilter} sx={{ m: 1 }}>
                        Clear
                    </Button>
                )}
            </Box>
            <Divider />
            <Box display="flex" justifyContent="center" alignItems="center" mt={2} mb={3}>
                <form onSubmit={handleSearch} style={{ display: 'flex', alignItems: 'center' }}>
                    <TextField
                        variant="outlined"
                        placeholder="Search meals..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        sx={{ borderRadius: '16px', marginRight: '10px' }}
                    />
                    <Button
                        type="submit"
                        variant="contained"
                        color="primary"
                        sx={{
                            backgroundColor: '#00695c',
                            '&:hover': {
                                backgroundColor: '#004d40',
                            },
                            color: '#fff',
                            padding: '10px 20px',
                            fontSize: '16px',
                            borderRadius: '16px',
                            boxShadow: '0 3px 5px rgba(0,0,0,0.2)',
                        }}
                    >
                        <SearchIcon />
                    </Button>
                </form>
            </Box>
            <Grid container spacing={2}>
                {paginatedMeals.length > 0 ? paginatedMeals.map((meal) => (
                    <Grid item key={meal.mealId} xs={12} sm={6} md={4}>
                        <Card sx={{ border: '1px solid #ddd', boxShadow: '0 2px 5px rgba(0,0,0,0.1)' }}>
                            <CardContent sx={{ textAlign: 'center' }}>
                                <Typography variant="h6" align="center" sx={{ backgroundColor: '#00695c', color: '#fff', padding: '8px 0' }}>
                                    {mealTypes[meal.mealType].label} <span role="img" aria-label="meal-icon">🍴</span>
                                </Typography>
                                <Typography variant="h6" component="div" align="center" sx={{ fontWeight: 'bold', margin: '10px 0' }}>
                                    {meal.mealName}
                                </Typography>
                                <CardMedia
                                    component="img"
                                    height="140"
                                    image={`https://localhost:44353/${meal.image}`}
                                    alt={meal.mealName}
                                    sx={{ margin: '10px auto' }}
                                />
                                <Button
                                    variant="contained"
                                    color="primary"
                                    onClick={() => handleNavigateToDetails(meal.mealId)}
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
                                    GO RECIPE
                                </Button>
                                <IconButton
                                    onClick={() => handleAddToPlanner(meal.mealId)}
                                    sx={{
                                        marginTop: '10px',
                                        color: isMealInPlanner(meal.mealId) ? '#ff4081' : '#ddd',
                                        '&:hover': {
                                            color: isMealInPlanner(meal.mealId) ? '#c60055' : '#ff4081',
                                        },
                                    }}
                                >
                                    {isMealInPlanner(meal.mealId) ? <FavoriteIcon /> : <FavoriteBorderIcon />}
                                </IconButton>
                            </CardContent>
                        </Card>
                    </Grid>
                )) : (
                    <Typography variant="h6" align="center" sx={{ marginTop: 4, color: 'red' }}>
                        No meals found.
                    </Typography>
                )}
            </Grid>
            <Box display="flex" justifyContent="center" mt={2}>
                <Pagination
                    count={totalPages}
                    page={pageNumber}
                    onChange={handlePageChange}
                />
            </Box>
        </Box>
    );
};

export default MealList;
