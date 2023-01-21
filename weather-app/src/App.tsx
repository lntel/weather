import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import "./index.css";

const App = () => {

    const { register, trigger, handleSubmit } = useForm();
    const [location, setLocation] = useState<string>('London');
    const [locations, setLocations] = useState<Location[]>([]);
    const [data, setData] = useState<any>();
    
    useEffect(() => {
        console.log(locations)
    }, [locations])
    

    const onKeyPressed = async (e: React.KeyboardEvent<HTMLInputElement>) => {
      if(e.key !== 'Enter') return;

      await getLocations();
    }

    const getLocations = async () => {
      const response = await fetch(`api/location/${location}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
      });

      console.log(response)

      if(!response.ok) return;

      const result = await response.json();

      setLocations(result);
    }

    const lookup = async (lat: number, lon: number, place: string) => {

      const response = await fetch(`api/weather?lat=${lat}&lon=${lon}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
      });

      if(!response.ok) return;

      setLocations([]);

        setData(await response.json());
    }

  return (
    <div className="bg-slate-900 max-w-full min-h-screen flex justify-center items-center flex-col">
      <form className="w-full px-10 sm:w-1/3" onSubmit={e => e.preventDefault()}>
      <h1 className="text-4xl text-primary font-light mb-5">Weather App</h1>
      <div className="bg-slate-700 rounded-md">
        <input type="text" autoComplete="address-level2" {...register('location')} onChange={e => setLocation(e.target.value)} onKeyUp={onKeyPressed} value={location} className="border-2 border-transparent focus:border-primary transition-all ease-in-out duration-300 bg-slate-800 py-4 pl-4 rounded-md w-full text-primary outline-none placeholder:text-slate-600 placeholder:font-semibold" placeholder="Enter a location"/>
        <ul className="py-2 px-5 text-white text-lg">
            {Boolean(locations && locations.length) && locations.map((d: any) => 
                <li onClick={() => lookup(d.lat, d.lon, `${d.name}, ${d.state}, ${d.country}`)} key={`${d.lat}/${d.lon}`} className="cursor-pointer">{d.name}, {d.state}, {d.country}</li>
            )}
        </ul>
      </div>
      </form>
      { Boolean(data) && (
      <div className="w-full mx-auto px-10 mt-5 sm:w-1/3">
        <div className="bg-slate-800 p-4 rounded-lg text-white">
          <h1 className="text-lg">{data?.name}</h1>
          <details className="bg-slate-700 p-3 rounded-lg mt-3">
            <summary>Temperature</summary>
            <label htmlFor="current_temp" className="text-primary text-sm uppercase">Current</label>
            <p id="current_temp">{data?.main?.temp} °C</p>
            <label htmlFor="max_temp" className="text-primary text-sm uppercase">Maximum</label>
            <p id="max_temp">{data?.main?.tempMax} °C</p>
            <label htmlFor="min_temp" className="text-primary text-sm uppercase">Minimum</label>
            <p id="min_temp">{data?.main?.tempMin} °C</p>
          </details>
            <label htmlFor="pressure" className="text-primary text-sm uppercase">Pressure</label>
            <p id="pressure">{data?.main?.pressure} hPa</p>
            <label htmlFor="humidity" className="text-primary text-sm uppercase">Humidity</label>
            <p id="humidity">{data?.main?.humidity}</p>
            <label htmlFor="sunrise" className="text-primary text-sm uppercase">Sunrise</label>
            <p id="sunrise">{new Date(data?.sys?.sunrise * 1000).toLocaleString()}</p>
            <label htmlFor="sunset" className="text-primary text-sm uppercase">Sunset</label>
            <p id="sunset">{new Date(data?.sys?.sunset * 1000).toLocaleString()}</p>
        </div>
      </div>
      ) }
    </div>
  );
};

export default App;
